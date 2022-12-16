namespace bs.Frmwrk.Core.ViewModels.Api
{
    public class ApiPagedResponse<T> : ApiResponse, IApiPagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}