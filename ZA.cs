public class ZA : IZA
{
    private const string zipExt = ".zip";
    public static ZA zip = new();

    public
#if ASYNC
    async Task
#else
        void
#endif
        CreateArchive(string root)
    {
#if ASYNC
        await
#endif
        CreateArchive(root, FilesFromRoot(root), FileFromRoot(root));
    }

    public
#if ASYNC
    async Task
#else
        void
#endif
        CreateArchive(string root, IEnumerable<string> soubory)
    {
#if ASYNC
        await
#endif
        CreateArchive(root, soubory, FileFromRoot(root));
    }

    public
#if ASYNC
async Task
#else
        void
#endif
        CreateArchive(string root, IEnumerable<string> soubory, string soubor)
    {
        //Path.WithEndSlash(ref root);

        byte[] compressedBytes;
        using MemoryStream outStream = new();
        using (ZipArchive archive = new(outStream, ZipArchiveMode.Create, true))
        {
            foreach (var item in soubory)
            {
                var fileInArchive = archive.CreateEntry(item.Replace(root, string.Empty), CompressionLevel.Optimal);
                using var entryStream = fileInArchive.Open();
                using MemoryStream fileToCompressStream = new(
#if ASYNC
await
#endif
                    TFSE.ReadAllBytesArray(item));
                fileToCompressStream.CopyTo(entryStream);
            }
        }

        compressedBytes = outStream.ToArray();

#if ASYNC
        await
#endif
        TFSE.WriteAllBytesArray(soubor, compressedBytes);
    }

    public
#if ASYNC
    async Task
#else
        void
#endif
        CreateArchive(string root, string souborZip)
    {
#if ASYNC
        await
#endif
        CreateArchive(root, FilesFromRoot(root), FileFromRoot(root));
    }

    public void ExtractArchive(string soubor, bool extractToSameFolder)
    {
        ExtractArchive(soubor, PathWithoutExtension(soubor), extractToSameFolder);
    }

    public void ExtractArchive(string archiveFilenameIn, string outFolder, bool extractToSameFolder)
    {
        if (extractToSameFolder)
            // Just one is right
            outFolder = Path.GetDirectoryName(outFolder);
        //outFolder = Path.GetDirectoryName(outFolder);
        ZipArchive zip = new(new FileStream(archiveFilenameIn, FileMode.Open));
        zip.ExtractToDirectory(outFolder, true);
    }

    private IEnumerable<string> FilesFromRoot(string root)
    {
        return Directory.GetFiles(root, "*.*", SearchOption.AllDirectories);
    }

    private string FileFromRoot(string root)
    {
        root = root.TrimEnd('\\');
        return root + zipExt;
    }

    public string PathWithoutExtension(string soubor)
    {
        return SHSE.TrimEnd(soubor, Path.GetExtension(soubor));
    }
}