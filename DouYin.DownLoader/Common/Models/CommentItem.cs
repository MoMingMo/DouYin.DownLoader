using MiniExcelLibs.Attributes;

namespace DouYin.DownLoader.Common.Models
{
    public class CommentItem
    {
        [ExcelColumnName("内容")]
        public string? Text { get; set; }
        [ExcelColumnName("点赞")]
        public int? DiggCount { get; set; }
        [ExcelColumnName("用户昵称")]
        public string? NickName { get; set; }
        [ExcelColumnName("用户昵称")]
        public DateTime CreateTime { get; set; }
    }
    public class CommentList 
    {
        public ICollection<CommentItem>? CommentItems { get; set; }
        public bool HasMore { get; set; } = true;
    }
}
