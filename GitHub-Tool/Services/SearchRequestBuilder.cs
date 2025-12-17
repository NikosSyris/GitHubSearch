using Octokit;
using System;
using System.Collections.Generic;
using GitHubSearch.Model;

namespace GitHubSearch.Services
{

    public class SearchRequestBuilder
    {

        public SearchCodeRequest BuildCodeSearchRequest(SearchCodeRequestParameters parameters)
        {
            ValidateParameters(parameters);

            var request = new SearchCodeRequest(parameters.Term);

            SetLanguage(request, parameters.Language);
            SetPath(request, parameters.Path);
            SetUser(request, parameters.Owner);
            SetFileName(request, parameters.FileName);
            SetExtension(request, parameters.Extension);
            SetForks(request, parameters.ForksIncluded);
            SetSize(request, parameters.SizeChoice, parameters.Size);
            SetInQualifier(request, parameters.PathIncluded);

            return request;
        }

        private void ValidateParameters(SearchCodeRequestParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (string.IsNullOrWhiteSpace(parameters.Term))
            {
                throw new ArgumentException(
                    "A search term is required for GitHub code search.",
                    nameof(parameters));
            }
        }

        private void SetLanguage(SearchCodeRequest request, string language)
        {
            if (string.IsNullOrWhiteSpace(language))
            {
                return;
            }

            if (Enum.TryParse<Language>(language, ignoreCase: true, out var parsedLanguage))
            {
                request.Language = parsedLanguage;
            }
        }

        private void SetPath(SearchCodeRequest request, string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                request.Path = path;
            }
        }

        private void SetUser(SearchCodeRequest request, string owner)
        {
            if (!string.IsNullOrWhiteSpace(owner))
            {
                request.User = owner;
            }
        }

        private void SetFileName(SearchCodeRequest request, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                request.FileName = fileName;
            }
        }

        private void SetExtension(SearchCodeRequest request, string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                request.Extension = extension;
            }
        }

        private void SetForks(SearchCodeRequest request, string forksIncluded)
        {
            // IMPORTANT: GitHub Code Search API only accepts fork:true or fork:only
            // Setting fork:false causes "unable to parse query" error!
            // Only set this property when explicitly including forks.
            if (!string.IsNullOrWhiteSpace(forksIncluded) &&
                forksIncluded.Equals("Yes", StringComparison.OrdinalIgnoreCase))
            {
                request.Forks = true;
            }
        }

        private void SetSize(SearchCodeRequest request, string sizeChoice, int size)
        {
            if (string.IsNullOrWhiteSpace(sizeChoice) || size <= 0)
            {
                return;
            }

            request.Size = sizeChoice.Equals("More than", StringComparison.OrdinalIgnoreCase)
                ? Range.GreaterThanOrEquals(size)
                : Range.LessThanOrEquals(size);
        }

        private void SetInQualifier(SearchCodeRequest request, bool? pathIncluded)
        {
            request.In = pathIncluded == true
                ? new[] { CodeInQualifier.File, CodeInQualifier.Path }
                : new[] { CodeInQualifier.File };
        }
    }
}