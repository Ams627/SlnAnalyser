using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SlnAnalyser
{
    class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                const string RegexGuid = @"{\s*[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}\s*}";
                const string RegexPathname = @"[a-z0-9-\\_\.]+";
                string RegexProject1 = $@"^Project\s*\(\s*\""\s*({RegexGuid})\s*\""\s*\)\s*=\s*";
                string RegexProject2 = $@"\""(\w+)\""\s*,\s*\""({RegexPathname})\""\s*,\s*\""{RegexGuid}\""";
                string RegexProject = RegexProject1 + RegexProject2;

                if (!args.Any())
                {
                    throw new Exception($"You must specify one or more solution files to analyse");
                }
                var plural = args.Length > 1;

                foreach (var filename in args)
                {
                    foreach (var line in File.ReadLines(filename).Select(x=>x.Trim()))
                    {
                        if (string.IsNullOrEmpty(line))
                        {
                            continue;
                        }

                        if (line.StartsWith("Project"))
                        {
                            var match = Regex.Match(line, RegexProject, RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                var groups = match.Groups.Count;
                                Console.WriteLine($"Matched - groups are {groups}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var fullname = System.Reflection.Assembly.GetEntryAssembly().Location;
                var progname = Path.GetFileNameWithoutExtension(fullname);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
