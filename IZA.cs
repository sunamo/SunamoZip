namespace SunamoZip;

public interface IZA
{
#if ASYNC
    Task
#else
void
#endif
    CreateArchive(string root);

#if ASYNC
    Task
#else
void
#endif
    CreateArchive(string slozka, IEnumerable<string> soubory);

#if ASYNC
    Task
#else
void
#endif
    CreateArchive(string root, IEnumerable<string> soubory, string soubor);

#if ASYNC
    Task
#else
void
#endif
    CreateArchive(string root, string souborZip);

    void ExtractArchive(string soubor, bool extractToSameFolder);
    void ExtractArchive(string archiveFilenameIn, string outFolder, bool extractToSameFolder);
}
