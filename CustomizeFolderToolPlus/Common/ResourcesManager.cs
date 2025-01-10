#nullable disable

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using static CustomizeFolderToolPlus.Common.Constants;

namespace CustomizeFolderToolPlus.Common;

public static class ResourcesManager
{
    private static class StringResource
    {
        public const nint RT_STRING = 6;

        public static byte[] GetData(int id, string value)
        {
            var ms = new MemoryStream();
            var w = new BinaryWriter(ms, Encoding.Default);
            WriteZero(w, id);
            w.Write((ushort)value.Length);
            w.Write(Encoding.Unicode.GetBytes(value));
            w.Close();
            var values = ms.ToArray();
            return values;
        }

        public static ushort GetBlockId(int stringId)
        {
            return (ushort)(stringId / 16 + 1);
        }

        private static void WriteZero(BinaryWriter w, int id)
        {
            for (var i = (GetBlockId(id) - 1) * 16; i < id; i++)
            {
                w.Write((ushort)0);
            }
        }
    }

    private static class IconResource
    {
        public const nint RT_ICON = 3;
        public const nint RT_GROUP_ICON = 14;

        // The first structure in an ICO file lets us know how many images are in the file.
        [StructLayout(LayoutKind.Sequential)]
        private struct ICONDIR
        {
            // Reserved, must be 0
            public ushort Reserved;

            // Resource type, 1 for icons.
            public ushort Type;

            // How many images.
            public ushort Count;
            // The native structure has an array of ICONDIRENTRYs as a final field.
        }

        // Each ICONDIRENTRY describes one icon stored in the ico file. The offset says where the icon image data
        // starts in the file. The other fields give the information required to turn that image data into a valid
        // bitmap.
        [StructLayout(LayoutKind.Sequential)]
        private struct ICONDIRENTRY
        {
            // The width, in pixels, of the image.
            public byte Width;

            // The height, in pixels, of the image.
            public byte Height;

            // The number of colors in the image; (0 if >= 8bpp)
            public byte ColorCount;

            // Reserved (must be 0).
            public byte Reserved;

            // Color planes.
            public ushort Planes;

            // Bits per pixel.
            public ushort BitCount;

            // The length, in bytes, of the pixel data.
            public int BytesInRes;

            // The offset in the file where the pixel data starts.
            public int ImageOffset;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct BITMAPINFOHEADER
        {
            public uint Size;
            public int Width;
            public int Height;
            public ushort Planes;
            public ushort BitCount;
            public uint Compression;
            public uint SizeImage;
            public int XPelsPerMeter;
            public int YPelsPerMeter;
            public uint ClrUsed;
            public uint ClrImportant;
        }

        // The icon in an exe/dll file is stored in a very similar structure:
        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        private struct GRPICONDIRENTRY
        {
            public byte Width;
            public byte Height;
            public byte ColorCount;
            public byte Reserved;
            public ushort Planes;
            public ushort BitCount;
            public int BytesInRes;
            public ushort ID;
        }

        public static (byte[] GroupData, byte[][] IconDatas) GetData(int baseId, byte[] value)
        {
            // Pin the bytes that can read as unmanaged data;
            var pinnedBytes = GCHandle.Alloc(value, GCHandleType.Pinned);
            // Read data as ICONDIR
            var iconDir = (ICONDIR)Marshal.PtrToStructure(pinnedBytes.AddrOfPinnedObject(), typeof(ICONDIR))!;
            // which tells us how many images are in the ico file. For each image, there's a ICONDIRENTRY, and associated pixel data.
            var iconEntry = new ICONDIRENTRY[iconDir.Count];
            var iconImage = new byte[iconDir.Count][];
            // The first ICONDIRENTRY will be immediately after the ICONDIR, so the offset to it is the size of ICONDIR
            var offset = Marshal.SizeOf(iconDir);
            // After reading an ICONDIRENTRY we step forward by the size of an ICONDIRENTRY            
            var iconDirEntryType = typeof(ICONDIRENTRY);
            var size = Marshal.SizeOf(iconDirEntryType);
            for (var i = 0; i < iconDir.Count; i++)
            {
                // Grab the structure.
                var entry =
                    (ICONDIRENTRY)Marshal.PtrToStructure(
                        new nint(pinnedBytes.AddrOfPinnedObject().ToInt64() + offset + size * i),
                        iconDirEntryType)!;
                iconEntry[i] = entry;
                // Grab the associated pixel data.
                iconImage[i] = new byte[entry.BytesInRes];
                Buffer.BlockCopy(value, entry.ImageOffset, iconImage[i], 0, entry.BytesInRes);
            }

            pinnedBytes.Free();

            // This will store the memory version of the icon.
            var sizeOfIconGroupData = Marshal.SizeOf(typeof(ICONDIR)) +
                                      Marshal.SizeOf(typeof(GRPICONDIRENTRY)) * iconDir.Count;
            var data = new byte[sizeOfIconGroupData];
            var pinnedData = GCHandle.Alloc(data, GCHandleType.Pinned);
            Marshal.StructureToPtr(iconDir, pinnedData.AddrOfPinnedObject(), false);
            size = Marshal.SizeOf(typeof(GRPICONDIRENTRY));
            for (var i = 0; i < iconDir.Count; i++)
            {
                var grpEntry = new GRPICONDIRENTRY();
                var bitmapheader = new BITMAPINFOHEADER();
                var pinnedBitmapInfoHeader = GCHandle.Alloc(bitmapheader, GCHandleType.Pinned);
                Marshal.Copy(iconImage[i], 0, pinnedBitmapInfoHeader.AddrOfPinnedObject(),
                    Marshal.SizeOf(typeof(BITMAPINFOHEADER)));
                pinnedBitmapInfoHeader.Free();
                grpEntry.Width = iconEntry[i].Width;
                grpEntry.Height = iconEntry[i].Height;
                grpEntry.ColorCount = iconEntry[i].ColorCount;
                grpEntry.Reserved = iconEntry[i].Reserved;
                grpEntry.Planes = bitmapheader.Planes;
                grpEntry.BitCount = bitmapheader.BitCount;
                grpEntry.BytesInRes = iconEntry[i].BytesInRes;
                grpEntry.ID = Convert.ToUInt16(baseId + i);
                Marshal.StructureToPtr(grpEntry,
                    new nint(pinnedData.AddrOfPinnedObject().ToInt64() + offset + size * i),
                    false);
            }

            pinnedData.Free();
            return (data, iconImage);
        }
    }

    public static void CreateStringResources(string resourceFile, int id, string value)
    {
        var handlePtr = BeginUpdateResource(resourceFile, false);

        if (handlePtr == nint.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        var values = StringResource.GetData(id, value);

        if (!UpdateResource(handlePtr, StringResource.RT_STRING, StringResource.GetBlockId(id), 0, values,
                (uint)values.Length))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        if (!EndUpdateResource(handlePtr, false))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    public static void DeleteTextResources(string resourceFile, int id)
    {
        var handlePtr = BeginUpdateResource(resourceFile, false);

        if (handlePtr == nint.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        if (!UpdateResource(handlePtr, StringResource.RT_STRING, StringResource.GetBlockId(id), 0, null, 0))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        if (!EndUpdateResource(handlePtr, false))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    public static void CreateIconResources(string resourceFile, int id, byte[] value)
    {
        var handleExe = BeginUpdateResource(resourceFile, false);

        if (handleExe == nint.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        var (groupData, iconDatas) = IconResource.GetData(id, value);
        if (!UpdateResource(handleExe, IconResource.RT_GROUP_ICON, id, 0, groupData,
                (uint)groupData.Length))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        for (var i = 0; i < iconDatas.Length; i++)
        {
            var iconData = iconDatas[i];
            if (!UpdateResource(handleExe, IconResource.RT_ICON, id + i, 0, iconData, (uint)iconData.Length))
                throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        if (!EndUpdateResource(handleExe, false))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    public static void DeleteIconResources(string resourceFile, int id)
    {
        var handleExe = BeginUpdateResource(resourceFile, false);

        if (handleExe == nint.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error());

        if (!UpdateResource(handleExe, IconResource.RT_ICON, id + 1, 0, null, 0))
            throw new Win32Exception(Marshal.GetLastWin32Error());

        if (!EndUpdateResource(handleExe, false))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    #region External

    [DllImport(Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern nint BeginUpdateResource(string fileName,
        [MarshalAs(UnmanagedType.Bool)] bool deleteExistingResources);

    [DllImport(Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UpdateResource(nint hUpdate, nint lpType, nint lpName, ushort wLanguage,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 5)]
        byte[] lpData,
        uint cbData);

    [DllImport(Kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EndUpdateResource(nint hUpdate, [MarshalAs(UnmanagedType.Bool)] bool discard);

    #endregion
}
