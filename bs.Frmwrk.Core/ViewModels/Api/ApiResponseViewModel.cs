namespace bs.Frmwrk.Core.ViewModels.Api
{
    public class ApiResponseViewModel : IApiResponseViewModel
    {
        public ApiResponseViewModel()
        {
            Success= true;
        }

        public ApiResponseViewModel(bool success, string errorMessage)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public long? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Success { get; set; }
        public string? WarnMessage { get; set; }
    }

    public class ApiResponseViewModel<T> : ApiResponseViewModel, IApiResponseViewModel<T>
    {
        public T Value { get; set; }
    }
}