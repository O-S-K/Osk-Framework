// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_EDITOR
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CandyCoded.GitStatus
{

    public static class GitIgnore
    {

        private const string URL = "https://raw.githubusercontent.com/github/gitignore/master/Unity.gitignore";

        private const string FILENAME = ".gitignore";

        public static async Task Create(string directory)
        {

            var path = Path.Combine(directory, FILENAME);

            if (File.Exists(path))
            {

                return;

            }

            var request = WebRequest.Create(URL);

            var response = await request.GetResponseAsync();

            var content = new MemoryStream();

            using (var stream = response.GetResponseStream())
            {

                if (stream != null)
                {

                    await stream.CopyToAsync(content);

                    File.WriteAllBytes(path, content.ToArray());

                }

            }

        }

    }

}
#endif
