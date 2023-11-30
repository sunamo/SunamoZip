public static class ZipArchiveExtensions
{
    public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
    {
        if (!overwrite)
        {
            archive.ExtractToDirectory(destinationDirectoryName);
            return;
        }

        var di = Directory.CreateDirectory(destinationDirectoryName);
        var destinationDirectoryFullPath = di.FullName;

        foreach (var file in archive.Entries)
        {
            var completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

            if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                throw new IOException(
                    "Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");

            if (file.Name == "")
            {
                // Assuming Empty for Directory
                Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                continue;
            }

            //createUpfoldersPsysicallyUnlessThere(completeFileName);
            File.Delete(completeFileName);
            file.ExtractToFile(completeFileName, true);
        }
    }
}