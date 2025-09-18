using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminBaseComponenets.BaseComs;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;

namespace AdminBaseComponenets.BaseComs
{
    public partial class FileBrowser : ComponentBase
    {
        [Parameter] public Func<string, Task> OnSuccess { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
        [Parameter] public string InitialPath { get; set; }

        // Bookmark logic
        protected List<string> QuickAccessDirs { get; set; } = new List<string>();
        private const string BookmarkStorageKey = "FileBrowserBookmarks";

        // Main state
        protected bool IsLoading = false;
        protected Models.FileBrowserResponse BrowserResponse;
        protected string CurrentPath = "";

        // Pagination
        protected int CurrentPage = 1;
        protected int PageSize = 50;
        protected int TotalPages => BrowserResponse != null && BrowserResponse.TotalCount > 0
            ? (int)Math.Ceiling((double)BrowserResponse.TotalCount / PageSize)
            : 1;

        // Directory creation
        protected bool ShowCreateDirectoryModal = false;
        protected string CreateDirectoryModalClass => ShowCreateDirectoryModal ? "show d-block" : "";
        protected string NewDirectoryName = "";
        protected bool IsCreatingDirectory = false;
        protected string CreateDirectoryError = "";
        protected bool CanCreateDirectory => !string.IsNullOrWhiteSpace(NewDirectoryName) && !IsCreatingDirectory;

        // Upload file modal
        protected bool ShowUploadFileModal = false;
        protected IBrowserFile SelectedUploadFile = null;
        protected bool IsUploadingFile = false;
        protected int UploadProgress = 0;
        protected string UploadFileError = "";

        // Search
        protected string _searchTerm = "";
        protected string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (_searchTerm != value)
                {
                    _searchTerm = value;
                    CurrentPage = 1;
                    _ = LoadFiles(CurrentPath, 1, PageSize, false);
                }
            }
        }

        [Inject] protected IJSRuntime JS { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadBookmarksFromStorage();
            await LoadFiles();
        }

        protected async Task LoadBookmarksFromStorage()
        {
            try
            {
                var json = await JS.InvokeAsync<string>("localStorage.getItem", BookmarkStorageKey);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    QuickAccessDirs = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                }
            }
            catch { QuickAccessDirs = new List<string>(); }
        }

        protected async Task SaveBookmarksToStorage()
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(QuickAccessDirs);
                await JS.InvokeVoidAsync("localStorage.setItem", BookmarkStorageKey, json);
            }
            catch { }
        }

        protected async Task AddQuickAccess()
        {
            if (!string.IsNullOrWhiteSpace(CurrentPath) && !QuickAccessDirs.Contains(CurrentPath))
            {
                QuickAccessDirs.Add(CurrentPath);
                await SaveBookmarksToStorage();
                StateHasChanged();
            }
        }

        protected void OpenUploadFileModal()
        {
            SelectedUploadFile = null;
            UploadFileError = "";
            UploadProgress = 0;
            ShowUploadFileModal = true;
            StateHasChanged();
        }

        protected void CloseUploadFileModal()
        {
            ShowUploadFileModal = false;
            SelectedUploadFile = null;
            UploadFileError = "";
            UploadProgress = 0;
            StateHasChanged();
        }

        protected void OnUploadFileInputChange(InputFileChangeEventArgs e)
        {
            SelectedUploadFile = e.File;
            UploadFileError = "";
            UploadProgress = 0;
        }

        protected async Task UploadFile()
        {
            if (SelectedUploadFile == null)
            {
                UploadFileError = "لطفا یک فایل انتخاب کنید.";
                return;
            }

            IsUploadingFile = true;
            UploadFileError = "";
            UploadProgress = 0;
            StateHasChanged();

            try
            {
                await FileUploader.instnace.upload2(
                    SelectedUploadFile,
                    CurrentPath,
                    (state) =>
                    {
                        UploadProgress = state.ProgressPercentage;
                        StateHasChanged();
                    },
                    async (ex) =>
                    {
                        Console.WriteLine($"فایل '{SelectedUploadFile.Name}' با موفقیت آپلود شد");
                        StateHasChanged();
                        CloseUploadFileModal();
                        await LoadFiles(CurrentPath);
                    },
                    async (ex) =>
                    {
                        UploadFileError = $"خطا در آپلود فایل: {ex.Message}";
                        StateHasChanged();
                        return new FileUploader.ExpetainOut { Retry = false };
                    }
                );
            }
            catch (Exception ex)
            {
                UploadFileError = $"خطا در آپلود فایل: {ex.Message}";
            }
            finally
            {
                IsUploadingFile = false;
                StateHasChanged();
            }
        }

        protected async Task LoadFiles(string path = null, int? page = null, int? pageSize = null, bool setReload = true)
        {
            if (setReload)
            {
                IsLoading = true;
                StateHasChanged();
            }

            try
            {
                var url = "api/file/browse";
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(path))
                {
                    queryParams.Add($"path={Uri.EscapeDataString(path)}");
                }
                int usePage = page ?? CurrentPage;
                int usePageSize = pageSize ?? PageSize;
                queryParams.Add($"page={usePage}");
                queryParams.Add($"pageSize={usePageSize}");
                if (!string.IsNullOrWhiteSpace(SearchTerm))
                {
                    queryParams.Add($"search={Uri.EscapeDataString(SearchTerm)}");
                }
                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }

                var response = await ClTool.WebClient.webClient.fetch<object, Models.FileBrowserResponse>(
                    url, System.Net.Http.HttpMethod.Get, null);

                if (response != null && response.done)
                {
                    BrowserResponse = response;
                    CurrentPath = response.CurrentPath;
                    if (page != null) CurrentPage = usePage;
                    if (pageSize != null) PageSize = usePageSize;
                }
                else
                {
                    Console.WriteLine(response?.text ?? "خطای نامشخص در بارگذاری فایل‌ها");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"خطا در بارگذاری فایل‌ها: {ex.Message}");
            }
            finally
            {
                if (setReload)
                    IsLoading = false;
                StateHasChanged();
            }
        }

        protected async Task NavigateToPath(string path)
        {
            CurrentPage = 1;
            _searchTerm = ""; // Reset search box when folder changes
            await LoadFiles(path, 1, PageSize);
        }

        protected async Task OnSuccessClicked(Models.FileBrowserItem item)
        {
            if (OnSuccess != null)
            {
                await OnSuccess(item.Path);
            }
        }

        protected async Task OnCancelClicked()
        {
            if (OnCancel.HasDelegate)
            {
                await OnCancel.InvokeAsync();
            }
        }

        // Audio playback state
        protected string PlayingAudioPath { get; set; } = null;
        protected ElementReference audioRef;

        protected void PlayAudio(Models.FileBrowserItem item)
        {
            PlayingAudioPath = item.Path;
            StateHasChanged();
        }

        protected bool IsPlayingAudio(Models.FileBrowserItem item)
        {
            return PlayingAudioPath == item.Path;
        }

        protected void OnAudioEnded()
        {
            PlayingAudioPath = null;
            StateHasChanged();
        }
        protected void OpenCreateDirectoryModal()
        {
            NewDirectoryName = "";
            CreateDirectoryError = "";
            ShowCreateDirectoryModal = true;
            StateHasChanged();
        }

        protected void CloseCreateDirectoryModal()
        {
            ShowCreateDirectoryModal = false;
            NewDirectoryName = "";
            CreateDirectoryError = "";
            StateHasChanged();
        }

        protected async Task CreateDirectory()
        {
            if (string.IsNullOrWhiteSpace(NewDirectoryName))
            {
                CreateDirectoryError = "نام پوشه نمی‌تواند خالی باشد";
                return;
            }

            IsCreatingDirectory = true;
            CreateDirectoryError = "";
            StateHasChanged();

            try
            {
                var url = $"api/file/createdir?path={Uri.EscapeDataString(CurrentPath ?? "")}&name={Uri.EscapeDataString(NewDirectoryName)}";

                var response = await ClTool.WebClient.webClient.fetch<object, Models.FileBrowserResponse>(
                    url, System.Net.Http.HttpMethod.Post, null);

                if (response != null && response.done)
                {
                    BrowserResponse = response;
                    CurrentPath = response.CurrentPath;
                    CloseCreateDirectoryModal();
                    Console.WriteLine($"پوشه '{NewDirectoryName}' با موفقیت ایجاد شد");
                }
                else
                {
                    CreateDirectoryError = response?.text ?? "خطا در ایجاد پوشه";
                }
            }
            catch (Exception ex)
            {
                CreateDirectoryError = $"خطا در ایجاد پوشه: {ex.Message}";
            }
            finally
            {
                IsCreatingDirectory = false;
                StateHasChanged();
            }
        }

        protected string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        protected async Task NextPage()
        {
            if (BrowserResponse != null && CurrentPage < TotalPages)
            {
                await LoadFiles(CurrentPath, CurrentPage + 1, PageSize);
            }
        }

        protected async Task PrevPage()
        {
            if (CurrentPage > 1)
            {
                await LoadFiles(CurrentPath, CurrentPage - 1, PageSize);
            }
        }

        protected async Task OnPageSizeChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newSize) && newSize > 0)
            {
                PageSize = newSize;
                CurrentPage = 1;
                await LoadFiles(CurrentPath, 1, PageSize);
            }
        }

        protected bool IsImageFile(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension)) return false;
            var ext = extension.Trim().ToLower();
            return ext == "jpg" || ext == "jpeg" || ext == "png" || ext == "gif" || ext == "bmp" || ext == "webp";
        }

        protected bool IsAudioFile(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension)) return false;
            var ext = extension.Trim().ToLower();
            return ext == "mp3" || ext == "ogg" || ext == "wav" || ext == "aac" || ext == "flac";
        }

        protected string GetThumbnailUrl(string path)
        {
            return $"{ClTool.WebClient.webClient.baseUrl}/Upload/{path}";
        }

        protected string GetAudioUrl(string path)
        {
            return $"{ClTool.WebClient.webClient.baseUrl}/Upload/{path}";
        }
    }
}
