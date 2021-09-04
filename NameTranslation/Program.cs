using System;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Strings;

namespace NameTranslation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using var env = GameEnvironment.Typical.Skyrim(SkyrimRelease.SkyrimSE);

            var skyrimModKey = ModKey.FromFileName("Skyrim.esm");
            var skyrim = env.LoadOrder.GetIfEnabledAndExists(skyrimModKey);
            var massParalysis = skyrim.Books.First(x => x.EditorID == "SpellTomeMassParalysis");
            ReportBook(skyrimModKey, massParalysis);
        }

        private static void ReportBook(ModKey modKey, IBookGetter book)
        {
            if (book.Name == null) return;
            var name = book.Name;
            
            Console.WriteLine($"Name for {book} from {modKey} is {name}");
            Console.WriteLine($"Had {book.Name.NumLanguages} registered languages");
                
            if (book.Name.TryLookup(Language.French, out var fre))
            {
                Console.WriteLine($"Had name in French: {fre}");
            }

            var outgoing = new SkyrimMod("Outgoing.esp", SkyrimRelease.SkyrimSE);
            var bookW = outgoing.Books.GetOrAddAsOverride(book);
            bookW.Name = $"{fre}Test";
            outgoing.WriteToBinary(outgoing.ModKey.FileName.ToString());
        }
    }
}
