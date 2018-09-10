using System.Collections.Generic;
using System.Threading.Tasks;
using Octokit;
using GitHub_Tool.Model;
using Model = GitHub_Tool.Model;

namespace GitHub_Tool.Action
{
    class RepoManager
    {

        public async Task<Model.Repository> getRepoStructure(Model.Repository repository)
        {
            Folder rootFolder = new Folder("root", "");
            var result = await getRepoContent(repository.Owner, repository.Name);

            foreach (var item in result)
            {
                if (item.Type == "dir")
                {
                    Folder newFolder = new Folder(item.Name, item.Path);
                    rootFolder.FolderList.Add(newFolder);
                    newFolder = await getFolderContent(repository, newFolder).ConfigureAwait(false);
                }
                else if (item.Type == "file")
                {
                    rootFolder.FileList.Add(new File(item.Name, repository.Owner, repository.Name, item.Path, item.HtmlUrl));
                    repository.Files.Add(new File(item.Name, repository.Owner, repository.Name, item.Path, item.HtmlUrl));
                }
            }

            repository.RootFolder = rootFolder;

            return repository;
        }


        private async Task<Folder> getFolderContent(Model.Repository repository, Folder folder)
        {

            var result = await getRepoContent(repository.Owner, repository.Name, folder.Path);

            foreach (var item in result)
            {
                if (item.Type == "file")
                {
                    folder.FileList.Add(new File(item.Name, repository.Owner, repository.Name, item.Path, item.HtmlUrl));
                    repository.Files.Add(new File(item.Name, repository.Owner, repository.Name, item.Path, item.HtmlUrl));
                }
                else if (item.Type == "dir")
                {
                    Folder newFolder = new Folder(item.Name, item.Path);
                    folder.FolderList.Add(newFolder);
                    newFolder = await getFolderContent(repository, newFolder).ConfigureAwait(false);
                }
            }

            return folder;
        }


        private async Task<IReadOnlyList<RepositoryContent>> getRepoContent(string owner, string name)
        {
            return await GlobalVariables.client.Repository.Content.GetAllContents(owner, name).ConfigureAwait(false);
        }


        private async Task<IReadOnlyList<RepositoryContent>> getRepoContent(string owner, string name, string path)
        {
            return await GlobalVariables.client.Repository.Content.GetAllContents(owner, name, path).ConfigureAwait(false);
        }
    }
}
