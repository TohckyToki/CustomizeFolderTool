using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizeFolderTool {
    public static class Desktop {
        public enum FolderType {
            Documents,
            MyDocuments,
            Pictures,
            MyPictures,
            PhotoAlbum,
            Music,
            MyMusic,
            MusicArtist,
            MusicAlbum,
            Videos,
            MyVideos,
            VideoAlbum,
            UseLegacyHTT,
            CommonDocuments,
            Generic,
        }

        public enum DefaultDropEffect {
            Copy = 1,
            Move = 2,
            CreateAshortcut = 4
        }

        public static class ShellClassInfo {
            public static string Name = "[.ShellClassInfo]";
            public static string LocalizedResourceName = "LocalizedResourceName";
        }
    }
}
