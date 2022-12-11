namespace bs.Frmwrk.Core.ViewModels.Api
{
    public class ApiPagedResponseViewModel<T> : ApiResponseViewModel, IApiPagedResponseViewModel<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
    }
}