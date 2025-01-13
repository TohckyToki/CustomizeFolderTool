using CustomizeFolderToolPlus.Common;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Reflection;
using static CustomizeFolderToolPlus.Common.Constants;

namespace CustomizeFolderToolPlus;

public class FolderTool
{

    private string FolderPath { get; }
    private string ToolFolder { get; }
    private string ResourceFolder { get; }
    private string DataConnection { get; }

    private bool IsNew { get; set; } = false;

    private uint ResourceId { get; set; }
    private int StringIndex { get; set; } = 1;
    private int IconIndex { get; set; } = 1;

    private string? ResourceFileFullName { get; set; }
    private string? ResourceFileRelativeName { get; set; }

    private FolderTool(string folderPath)
    {
        this.FolderPath = folderPath;
        this.ToolFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        this.ResourceFolder = Path.Combine(this.ToolFolder, ToolResourceFolder);

        this.DataConnection = string.Format("Data Source={0}", Path.Combine(this.ToolFolder, "Data.dll"));
    }

    private uint GetLastId()
    {
        using var cnn = new SqliteConnection(DataConnection);
        cnn.Open();

        using var cmd = new SqliteCommand("select Id from Log order by Id desc limit 1;", cnn);
        using var reader = cmd.ExecuteReader();
        var maxIndex = reader.Read() ? reader.GetFieldValue<uint>(0) + 1u : 1u;

        cnn.Close();

        this.IsNew = true;

        return maxIndex;
    }

    private bool GetLogInfo(uint id)
    {
        var result = false;

        using var cnn = new SqliteConnection(DataConnection);
        cnn.Open();

        using var cmd = new SqliteCommand("select StringIndex, IconIndex from Log where Id = @id;", cnn);
        cmd.Parameters.Add(new SqliteParameter("@id", id));
        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            this.StringIndex = reader.GetFieldValue<int>(0) + 1;
            this.IconIndex = reader.GetFieldValue<int>(1) + 1;

            result = true;
        }
        cnn.Close();

        return result;
    }

    private void GenerateResourceFile()
    {
        if (!Directory.Exists(this.ResourceFolder))
        {
            Directory.CreateDirectory(this.ResourceFolder);
        }

        this.ResourceId = FolderManager.GetFlags(this.FolderPath);
        if (this.ResourceId <= 0 || !this.GetLogInfo(this.ResourceId))
        {
            this.ResourceId = this.GetLastId();
        }

        this.ResourceFileFullName = Path.Combine(this.ResourceFolder, string.Format(ToolResourceTemplateFileName, this.ResourceId));
        if (!File.Exists(this.ResourceFileFullName))
        {
            File.Copy(Path.Combine(this.ToolFolder, ToolResourceFileName), this.ResourceFileFullName);
        }

        this.ResourceFileRelativeName = Path.Combine(BaseFolder, ToolResourceFolder, Path.GetFileName(this.ResourceFileFullName));
    }

    private void UpdateStringIndex()
    {
        using var cnn = new SqliteConnection(DataConnection);
        cnn.Open();

        using var cmd = this.IsNew ?
            new SqliteCommand("insert into Log(StringIndex, IconIndex, Id) VALUES (@si, 0, @id);", cnn)
            : new SqliteCommand("update Log set StringIndex = @si where Id = @id;", cnn);
        cmd.Parameters.Add(new SqliteParameter("@si", this.StringIndex));
        cmd.Parameters.Add(new SqliteParameter("@id", this.ResourceId));
        var result = cmd.ExecuteNonQuery();

        cnn.Close();
    }

    private void UpdateIconIndex()
    {
        using var cnn = new SqliteConnection(DataConnection);
        cnn.Open();

        using var cmd = this.IsNew ?
            new SqliteCommand("insert into Log(StringIndex, IconIndex, Id) VALUES (0, @ii, @id);", cnn)
            : new SqliteCommand("update Log set IconIndex = @ii where Id = @id;", cnn);
        cmd.Parameters.Add(new SqliteParameter("@ii", this.IconIndex));
        cmd.Parameters.Add(new SqliteParameter("@id", this.ResourceId));
        var result = cmd.ExecuteNonQuery();

        cnn.Close();
    }

    public void CreateAlias(string alias)
    {
        if (this.ResourceFileFullName != null && this.ResourceFileRelativeName != null)
        {
            ResourcesManager.CreateStringResources(this.ResourceFileFullName, this.StringIndex, alias);
            FolderManager.SetLocalizedName(this.FolderPath, this.ResourceFileRelativeName, this.StringIndex);
            FolderManager.SetFlags(this.FolderPath, this.ResourceId);
            this.UpdateStringIndex();
        }
    }

    public void DeleteAlias()
    {
        FolderManager.RemoveLocalizedName(this.FolderPath);
    }

    public void CreateIcon(byte[] iconData)
    {
        if (this.ResourceFileFullName != null && this.ResourceFileRelativeName != null)
        {
            ResourcesManager.CreateIconResources(this.ResourceFileFullName, this.IconIndex, iconData);
            FolderManager.SetIcon(this.FolderPath, this.ResourceFileRelativeName, this.IconIndex * -1);
            FolderManager.SetFlags(this.FolderPath, this.ResourceId);
            this.UpdateIconIndex();
        }
    }

    public void DeleteIcon()
    {
        FolderManager.RemoveIcon(this.FolderPath);
    }

    public void CreateComment(string tipinfo)
    {
        FolderManager.SetInfoTip(this.FolderPath, tipinfo);
        FolderManager.SetFlags(this.FolderPath, this.ResourceId);
    }

    public void DeleteComment()
    {
        FolderManager.RemoveInfoTip(this.FolderPath);
    }

    private void Reset()
    {
        this.ResourceId = FolderManager.GetFlags(this.FolderPath);
        if (this.ResourceId > 0)
        {
            var resourceFileFullName = Path.Combine(this.ResourceFolder, string.Format(ToolResourceTemplateFileName, this.ResourceId));
            if (File.Exists(resourceFileFullName))
            {
                File.Delete(resourceFileFullName);
            }
            var desktopFile = Path.Combine(this.FolderPath, DesktopFile);
            if (File.Exists(desktopFile))
            {
                File.Delete(desktopFile);
            }

            FolderManager.SetFlags(this.FolderPath, this.ResourceId);
        }
    }

    public static FolderTool Create(string folderPath)
    {
        var tool = new FolderTool(folderPath);
        tool.GenerateResourceFile();
        return tool;
    }

    public static void Reset(string folderPath)
    {
        var tool = new FolderTool(folderPath);
        tool.Reset();
    }

    public static void Reapply(string folderPath)
    {
        var desktopFile = Path.Combine(folderPath, DesktopFile);
        if (File.Exists(desktopFile))
        {
            FolderManager.RefreshFolder(folderPath);
        }
    }
}
