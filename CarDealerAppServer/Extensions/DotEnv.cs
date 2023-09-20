namespace CarDealerAppServer.Api.Extensions
{
    public class DotEnv
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }
            foreach(var line in File.ReadLines(filePath))
            {
                var index = line.IndexOf('=');
                if (index == -1)
                {
                    continue;
                }
                var name = line[..index];
                var value = line[(index + 1)..];
                Environment.SetEnvironmentVariable(name, value);
            }
        }
    }
}
