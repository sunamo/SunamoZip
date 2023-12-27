
namespace ad;
/// <summary>
/// M�lo by tu v�echno fungovat, str�vil jsem nad t�m hezk�ch p�r hodin :(
/// </summary>
public class ZA : IZA
{
    #region IArchiv Members
    public static ZA zip = new ZA();

    /// <summary>
    /// Kdy� komprimuji celou slo�ku A1, vr�t� mi arch�v zip jm�na slo�ky A1 ve slo�ce ve kter� je A1.
    /// </summary>
    /// <param name="root"></param>
    private string VratJmenoSouboruZip(string root)
    {
        string soubor = Path.Combine(Path.GetDirectoryName(root), Path.GetFileNameWithoutExtensionLower(root) + ".zip");
        return soubor;
    }

    private string getRelativePath2(string filePath, string basePath)
    {
        if (!basePath.EndsWith(AllStringsSE.bs))
        {
            basePath += AllStringsSE.bs;
        }
        return filePath.Replace(basePath, "");
    }

    /// <summary>
    /// Ulozi soubory A2 do A3.
    /// A1 je k tomu, aby se mohla zjistit relativni cesta.
    /// </summary>
    /// <param name="root"></param>
    /// <param name="soubory"></param>
    /// <param name="soubor"></param>
    public void CreateArchive(string root, List<string> soubory, string soubor)
    {
        if (!HasRightExtension(soubor))
        {
            return;
        }
        #region Nac. jedn soubory a ukkladam do ZipOutputStream
        using (ZipOutputStream s = new ZipOutputStream(File.Create(soubor)))
        {
            s.SetLevel(9);
            for (int i = 0; i < soubory.Count; i++)
            {
                string var = getRelativePath2(soubory[i], root);
                ZipEntry ze = new ZipEntry(var);
                //ze.IsFile = true;

                s.PutNextEntry(ze);

                try
                {
                    FileStream fs = new FileStream(Path.Combine(root, var), FileMode.Open);
                    byte[] fero = new byte[fs.Length];
                    fs.Read(fero, 0, (int)fs.Length);
                    fs.Flush();
                    fs.Close();

                    fs.Dispose();
                    fs = null;
                    List<byte> b = new List<byte>(fero);

                    foreach (byte var2 in b)
                    {
                        s.WriteByte(var2);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                }
            }
            s.Flush();
            s.Finish();
        }
        #endregion
    }



    static Type type = typeof(ZA);

    private bool HasRightExtension(string soubor)
    {
        return ThrowEx.WrongExtension(soubor, AllExtensions.zip);
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="root"></param>
    /// <param name="p"></param>
    private string OdstranZJmenaSouboru(string root, string p)
    {
        string c = Path.GetDirectoryName(root);
        string s = Path.GetFileName(root).Replace(p, "");
        return Path.Combine(c, s);
    }


    #endregion

    public void CreateArchive(string root)
    {
        CreateArchive(root, VratSouboryRek(root), VratJmenoSouboruZip(root));
    }

    private List<string> VratSouboryRek(string root)
    {
        return Path.GetFiles(root, AllStringsSE.asterisk, SearchOption.AllDirectories);
    }

    public void CreateArchive(string slozka, List<string> soubory)
    {
        CreateArchive(slozka, soubory, VratJmenoSouboruZip(slozka));
    }

    public void CreateArchive(string root, string souborZip)
    {

        CreateArchive(root, VratSouboryRek(root), souborZip);
    }

    public void ExtractArchive(string archiveFilenameIn, string outFolder)
    {
        if (!HasRightExtension(archiveFilenameIn))
        {
            return;
        }
        if (!Path.ExistsDirectory(outFolder))
        {
            Path.CreateDirectory(outFolder);
        }
        ZipFile zf = null;
        try
        {
            FileStream fs = File.OpenRead(archiveFilenameIn);
            zf = new ZipFile(fs);
            foreach (ZipEntry zipEntry in zf)
            {
                String entryFileName = zipEntry.Name;
                byte[] buffer = new byte[4096];     // 4K is optimum
                Stream zipStream = zf.GetInputStream(zipEntry);

                entryFileName = entryFileName.TrimEnd(AllCharsSE.slash).Replace(AllCharsSE.slash, AllCharsSE.bs);

                // Manipulate the output filename here as desired.
                String fullZipToPath = Path.Combine(outFolder, entryFileName).TrimEnd(AllCharsSE.bs);
                string directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                    Path.CreateDirectoryIfNotExists(directoryName);

                using (FileStream streamWriter = File.Create(fullZipToPath))
                {
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
                }
            }
        }
        finally
        {
            if (zf != null)
            {
                zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                zf.Close(); // Ensure we release resources
            }
        }
    }

    public void ExtractArchive(string soubor)
    {
        ExtractArchive(soubor, OdstranZJmenaSouboru(soubor, ".zip"));
    }
}
