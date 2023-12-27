using SunamoZip.Extensions;

namespace SunamoZip;

public class SevenZipHelper : IZA
{
    private const string template = "7z.exe a -r \"{0}\" \"{1}{2}\"";

    public
#if ASYNC
    async Task
#else
void
#endif
    CreateArchive(string slozku)
    {
    }

    public
#if ASYNC
    async Task
#else
void
#endif
    CreateArchive(string slozka, IEnumerable<string> soubory)
    {
    }

    public
#if ASYNC
    async Task
#else
void
#endif
    CreateArchive(string slozku, IEnumerable<string> soubory, string soubor)
    {
    }

    public
#if ASYNC
    async Task
#else
void
#endif
    CreateArchive(string slozku, string souborZip)
    {
    }

    public void ExtractArchive(string soubor, bool extractToSameFolder)
    {
        ExtractArchive(soubor, ZA.zip.PathWithoutExtension(soubor), extractToSameFolder);
    }

    public void ExtractArchive(string archiveFilenameIn, string outFolder, bool extractToSameFolder)
    {
        if (extractToSameFolder)
        {
            outFolder = Path.GetDirectoryName(outFolder);
            outFolder = Path.GetDirectoryName(outFolder);
        }

        //using (var input = File.OpenRead(lstFiles[0]))
        //{
        //    using (var ds = new SevenZipExtractor(input))
        //    {
        //        //ds.ExtractionFinished += DsOnExtractionFinished;

        //        var mem = new MemoryStream();
        //        ds.ExtractFile(0, mem);

        //        using (var sr = new StreamReader(mem))
        //        {
        //            var iCount = 0;
        //            String line;
        //            mem.Position = 0;
        //            while ((line = sr.ReadLine()) != null && iCount < 100)
        //            {
        //                iCount++;
        //                //LstOutput.Items.Add(line);
        //            }

        //        }
        //    }
        //}

        //using (SevenZipArchive archive = new SevenZipArchive("Sample.7z"))
        //{
        //    // Extract all file in 7zip to a directory using ExtractToDirectory method.
        //    archive.ExtractToDirectory(dataDir + "Sample_ExtractionFolder");
        //}

        ZipArchive zip = new(new FileStream(archiveFilenameIn, FileMode.Open));
        zip.ExtractToDirectory(outFolder, true);
    }

    public static string CreateArchive(string pathOutput, string folder, string masc)
    {
        return string.Format(template, pathOutput, folder, masc);
    }

    public static string CreateArchiveInUpFolder(string fullPathFolder, string masc)
    {
        var upfolder = Path.GetDirectoryName(fullPathFolder);
        var zipPath = Path.Combine(upfolder, Path.GetFileName(fullPathFolder) + ".7z");

        return CreateArchive(zipPath, fullPathFolder, masc);
    }
}
