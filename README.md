# GitHub-Search

GitHub Search Tool
A Windows desktop application for searching code and repositories across GitHub with advanced filtering options. Built with C#, WPF and the Octokit.NET library.
Key capabilities:

Search code and repositories with powerful filters
Download search results for offline analysis
Download multiple versions (commits) of any file to track its evolution over time
Browse repository structure with direct links to every file


🎓 This project was developed as a diploma thesis. It focuses on functionality and API integration rather than visual design — so please don't judge the UI too harshly! It may not win any beauty contests, but it gets the job done. 😄

<img width="1163" height="880" alt="image" src="https://github.com/user-attachments/assets/80589dac-5d9f-4cfc-b40c-51310e678b76" />


## Features

### Code Search
- **Search for code** across all public GitHub repositories
- **Advanced filtering** by:
  - Programming language
  - File extension
  - File name
  - Repository owner/organization
  - File path
  - File size
  - Fork inclusion

### Repository Search
- **Search for repositories** with filters for:
  - Programming language
  - Stars count
  - Forks count
  - Repository size
  - Creation date / date range
  - Last updated date
  - README content inclusion

### Additional Features
- **Commit History**: View the complete commit history for any file in the search results
- **Version Comparison**: Browse and view specific versions of files at different commits
- **Download Files**: Download specific file versions to your local machine
- **Export Results**: Save search results to a text file
- **Pagination**: Navigate through large result sets with built-in pagination support

## Screenshots

### Code Search Results

<img width="1891" height="1031" alt="image" src="https://github.com/user-attachments/assets/37dc0165-30ec-4dc6-9be4-fa377a32a965" />

### Download the results

<img width="775" height="305" alt="image" src="https://github.com/user-attachments/assets/8c32c83d-f038-43d6-af20-3573a9c89415" />


<img width="1894" height="453" alt="image" src="https://github.com/user-attachments/assets/0e4403d4-5535-459b-801e-3a8cc91d0234" />

### Show the commits and download all of them or just a subset


<img width="777" height="300" alt="image" src="https://github.com/user-attachments/assets/168544a7-c437-424d-8e9d-7946bd025c75" />


<img width="781" height="170" alt="image" src="https://github.com/user-attachments/assets/23bba36d-a5a5-4e78-acf7-29d57e9588b1" />


### Repository Search Results


<img width="1894" height="822" alt="image" src="https://github.com/user-attachments/assets/91d8bc8b-c9c8-41eb-8141-0ad654bc305e" />


### Get the repositories structure and a table containing all the projects' files


<img width="1710" height="1016" alt="image" src="https://github.com/user-attachments/assets/ddced939-2b09-4cc0-8a8b-59c3968611bb" />


### Commit History View


<img width="782" height="304" alt="image" src="https://github.com/user-attachments/assets/a41a61f7-fee7-4ee0-acbf-a57c38a87318" />



## Prerequisites

- Windows 7 or later
- .NET Framework 4.6.1 or later
- A GitHub Personal Access Token (for API access)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/NikosSyris/GitHubSearch.git
   ```

2. Open `GitHubSearch.sln` in Visual Studio 2017 or later

3. Restore NuGet packages (right-click solution → Restore NuGet Packages)

4. Build the solution (Ctrl + Shift + B)

5. Run the application (F5)

## Getting a GitHub Personal Access Token

The application requires a GitHub Personal Access Token to access the GitHub API.

1. Go to [GitHub Settings → Developer settings → Personal access tokens](https://github.com/settings/tokens)
2. Click **"Generate new token (classic)"**
3. Give your token a descriptive name (e.g., "GitHub Search Tool")
4. Select the following scopes:
   - `public_repo` - Access public repositories
   - `repo` (optional) - Access private repositories
5. Click **"Generate token"**
6. Copy the token and paste it into the application when prompted

> ⚠️ **Important**: Keep your token secure and never commit it to version control!

## Usage

### Code Search

1. Launch the application
2. Click **"Search Code"** to open the code search tab
3. Enter your GitHub Personal Access Token
4. Enter a search term in the **"Term"** field
5. (Optional) Set additional filters:
   - **Language**: Filter by programming language (e.g., CSharp, Python, JavaScript)
   - **Extension**: Filter by file extension (e.g., cs, py, js)
   - **Owner**: Search within a specific user's or organization's repositories
   - **File Name**: Search for specific file names
   - **Path**: Search within a specific directory path
   - **Size**: Filter by file size (greater than or less than specified KB)
   - **Forks Included**: Choose whether to include forked repositories
6. Click **Search**

### Repository Search

1. Click **"Search Repos"** to open the repository search tab
2. Enter your GitHub Personal Access Token
3. Enter a search term
4. (Optional) Set filters for stars, forks, size, dates, language, etc.
5. Click **Search**

### Viewing Commit History

1. Perform a code search to get results
2. Select a file from the results grid
3. Click **"Show Commits"**
4. The commit history window will display all commits that modified this file
5. Select commits you want to download and click **"Download"**

### Downloading Files

1. In the Commit History window, check the commits you want to download
2. Specify the download destination folder
3. Click **"Download"**
4. Files are saved with timestamps in the filename for easy version comparison

### Exporting Search Results

1. After performing a search, click **"Download Results"**
2. Specify the destination folder and filename
3. Results are saved as a tab-separated text file

## Project Structure

```
GitHubSearch/
├── GitHub-Tool/
│   ├── Action/
│   │   ├── CodeSearchManager.cs       # Handles GitHub code search operations
│   │   ├── RepoSearchManager.cs       # Handles GitHub repository search operations
│   │   ├── RepoManager.cs             # Handles repository content operations
│   │   ├── DownloadLocallyManager.cs  # Handles file downloads
│   │   └── Validation/
│   │       └── DateValidator.cs       # Date validation utilities
│   ├── Model/
│   │   ├── Commit.cs                  # Commit data model
│   │   ├── File.cs                    # File search result model
│   │   ├── Folder.cs                  # Folder structure model
│   │   ├── Repository.cs              # Repository data model
│   │   ├── RequestParameters.cs       # Base request parameters
│   │   ├── SearchCodeRequestParameters.cs        # Code search parameters
│   │   └── SearchRepositoriesRequestParameters.cs # Repo search parameters
│   ├── Services/
│   │   ├── GitHubClientService.cs     # GitHub client management
│   │   └── SearchRequestBuilder.cs    # Builds search requests
│   ├── GUI/
│   │   ├── MainWindow.xaml(.cs)       # Main application window
│   │   ├── SearchCodeUserControl.xaml(.cs)    # Code search UI
│   │   ├── SearchRepoUserControl.xaml(.cs)    # Repository search UI
│   │   ├── CommitWindow.xaml(.cs)     # Commit history window
│   │   ├── DownloadResultsWindow.xaml(.cs)    # Download results window
│   │   ├── RepositoryWindow.xaml(.cs) # Repository structure window
│   │   └── PageDialog.xaml(.cs)       # Pagination dialog
│   └── App.xaml(.cs)                  # Application entry point
├── GitHubSearch.sln                   # Visual Studio solution file
└── README.md                          # This file
```

## Architecture

The application follows a clean separation of concerns:

- **Services Layer** (`Services/`): Handles GitHub API client management and authentication
- **Action Layer** (`Action/`): Contains business logic for searching and downloading
- **Model Layer** (`Model/`): Data models and request parameters
- **GUI Layer** (`GUI/`): WPF user interface components

### Key Design Patterns

- **Dependency Injection**: Services are injected into managers for better testability
- **Builder Pattern**: `SearchRequestBuilder` constructs complex search requests
- **Async/Await**: All API calls are asynchronous for responsive UI

## API Rate Limits

GitHub imposes rate limits on API requests:

| Authentication | Search Requests | Core Requests |
|----------------|-----------------|---------------|
| With Token     | 30 per minute   | 5,000 per hour |
| Without Token  | 10 per minute   | 60 per hour |

The application requires authentication to ensure reasonable rate limits.

## Dependencies

- [Octokit.NET](https://github.com/octokit/octokit.net) - GitHub API client library for .NET
- .NET Framework 4.6.1+
- WPF (Windows Presentation Foundation)

## Known Limitations

- Maximum of 1000 results per search query (GitHub API limitation)
- Code search requires at least one search term
- Some advanced GitHub search qualifiers may not be supported
- The UI is... functional 🙃 (beauty is in the eye of the beholder!)

## Troubleshooting

### "Unable to parse query" Error

This error occurs when the search query is malformed. Common causes:
- Empty search term (a search term is required)
- Invalid filter combinations
- Using `fork:false` (not supported by GitHub API)

### "Rate limit exceeded" Error

You've exceeded GitHub's API rate limits. Wait for the rate limit to reset (usually 1 minute for search, 1 hour for other requests).

### "Bad credentials" Error

Your Personal Access Token is invalid or has expired. Generate a new token and try again.

### No Results Found

- Try broadening your search criteria
- Check that your filters are not too restrictive
- Verify that the language/extension names are spelled correctly

### Files Not Downloading

- Ensure you have write permissions to the download destination
- Check that the destination folder exists
- Try running the application as administrator

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Ideas for Improvement

- 🎨 Improve the UI/UX design
- 📊 Add data visualization for search results
- 🔄 Add automatic pagination
- 💾 Add caching for repeated searches
- 🌙 Add dark mode support

## License

This project is open source. See the repository for license details.

## Author

**Nikos Syris** - [GitHub Profile](https://github.com/NikosSyris)

*Developed as a diploma thesis project*

## Acknowledgments

- [Octokit.NET](https://github.com/octokit/octokit.net) - For the excellent GitHub API wrapper
- [GitHub API Documentation](https://docs.github.com/en/rest) - For comprehensive API documentation
- My thesis advisor and reviewers
