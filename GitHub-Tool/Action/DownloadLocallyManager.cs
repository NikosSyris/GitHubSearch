using Octokit;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using GitHubSearch.Services;

namespace GitHubSearch.Action
{
    class DownloadLocallyManager
    {

        public string DefaultDownloadDestination { get; } = @"c:\";
        public string DefaultName { get; } = "result";

        private readonly GitHubClientService _clientService;
        private CodeSearchManager _codeSearchManager;

        public DownloadLocallyManager(GitHubClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
            _codeSearchManager = new CodeSearchManager(_clientService);
        }

        public async Task DownloadFileContentAsync(IEnumerable commits, string downloadDestination)
        {
            foreach (Model.Commit commit in commits)
            {
                if (commit.IsSelected == true)
                {
                    try
                    {
                        var file = await _codeSearchManager.GetFileAtCommitAsync(
                            commit.Owner,
                            commit.RepoName,
                            commit.FilePath,
                            commit.Sha
                        ).ConfigureAwait(false);

                        if (file == null)
                        {
                            continue;
                        }

                        string path = CreateFolder(commit, file.Name, downloadDestination);

                        string content = file.Content ?? "";

                        using (StreamWriter streamWriter = new StreamWriter(path))
                        {
                            streamWriter.WriteLine(file.HtmlUrl);
                            streamWriter.WriteLine(commit.Sha);
                            streamWriter.WriteLine();
                            streamWriter.Write(content);
                        }
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }
                }
            }
        }


        public void DownloadAllSearchResults(Model.RequestParameters requestParameters, ItemCollection items,
                                             string downloadDestination, string fileName)
        {
            if (!DirectoryExists(downloadDestination))
            {
                throw new DirectoryNotFoundException();
            }

            string pathString = downloadDestination;
            pathString = Path.Combine(pathString, fileName);

            try
            {
                using (TextWriter textWriter = new StreamWriter(pathString))
                {
                    textWriter.WriteLine(requestParameters.ToString() + DateTime.Now + "\r\n");

                    foreach (var item in items)
                    {
                        if (item is Model.File)
                        {
                            var file = item as Model.File;
                            textWriter.WriteLine(file.Name + "\t" + file.Owner + "\t" + file.RepoName + "\t" + file.HtmlUrl);
                        }
                        else
                        {
                            var repo = item as Model.Repository;
                            textWriter.WriteLine(repo.Name + "\t" + repo.Owner + "\t" + repo.Size + "\t" + repo.Language + "\t"
                                                 + repo.CreatedAt + "\t" + repo.UpdatedAt + "\t" + repo.StargazersCount + "\t"
                                                 + repo.ForksCount + "\t" + repo.HtmlUrl + "\t" + repo.Description);
                        }
                    }

                    textWriter.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
        }


        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }


        private string CreateFolder(Model.Commit commit, string fileName, string downloadDestination)
        {
            // Create folder structure: destination/repoName by owner/
            string folderPath = Path.Combine(downloadDestination, commit.RepoName + " by " + commit.Owner);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create the full file path with timestamp and order
            // Format: originalName_yyyy-MM-dd--HH-mm-ss_order.extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string timestamp = commit.CreatedAt.DateTime.ToString("yyyy-MM-dd--HH-mm-ss");

            string newFileName = $"{fileNameWithoutExtension}_{timestamp}_{commit.Order}{extension}";
            string fullPath = Path.Combine(folderPath, newFileName);

            return fullPath;
        }
    }
}