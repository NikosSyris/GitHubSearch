using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using GitHubSearch.Model;
using GitHubSearch.Services;

namespace GitHubSearch.Action
{
    class RepoManager
    {
        private readonly GitHubClientService _clientService;

        public RepoManager(GitHubClientService clientService)
        {
            _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
        }

        public async Task<Model.Repository> GetRepositoryStructureAsync(Model.Repository repository)
        {
            Folder rootFolder = new Folder("root", "");
            var result = await GetRepositoryContentAsync(repository.Owner, repository.Name).ConfigureAwait(false);

            foreach (var item in result)
            {
                if (item.Type == ContentType.Dir)
                {
                    Folder newFolder = new Folder(item.Name, item.Path);
                    rootFolder.FolderList.Add(newFolder);
                    newFolder = await GetFolderContentAsync(repository, newFolder).ConfigureAwait(false);
                }
                else if (item.Type == ContentType.File)
                {
                    var file = new File(item.Name, repository.Owner, repository.Name, item.Path, item.HtmlUrl);
                    rootFolder.FileList.Add(file);
                    repository.Files.Add(file);
                }
            }

            repository.RootFolder = rootFolder;
            return repository;
        }

        private async Task<Folder> GetFolderContentAsync(Model.Repository repository, Folder folder)
        {
            var result = await GetRepositoryContentAsync(repository.Owner, repository.Name, folder.Path).ConfigureAwait(false);

            foreach (var item in result)
            {
                if (item.Type == ContentType.File)
                {
                    var file = new File(item.Name, repository.Owner, repository.Name, item.Path, item.HtmlUrl);
                    folder.FileList.Add(file);
                    repository.Files.Add(file);
                }
                else if (item.Type == ContentType.Dir)
                {
                    Folder newFolder = new Folder(item.Name, item.Path);
                    folder.FolderList.Add(newFolder);
                    newFolder = await GetFolderContentAsync(repository, newFolder).ConfigureAwait(false);
                }
            }

            return folder;
        }

        private async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContentAsync(string owner, string name)
        {
            return await _clientService.Client.Repository.Content
                .GetAllContents(owner, name)
                .ConfigureAwait(false);
        }

        private async Task<IReadOnlyList<RepositoryContent>> GetRepositoryContentAsync(string owner, string name, string path)
        {
            return await _clientService.Client.Repository.Content
                .GetAllContents(owner, name, path)
                .ConfigureAwait(false);
        }
    }
}