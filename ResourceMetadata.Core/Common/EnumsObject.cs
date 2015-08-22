using System.ComponentModel.DataAnnotations;

namespace ResourceMetadata.Core.Common
{
    public enum SortType
    {
        ASC,
        DESC
    }

    public enum CategoryType
    {
        [Display(Name = "System Category")]
        SysCategory,
        [Display(Name = "Shop Category")]
        ShopCategory,
        [Display(Name = "System Menu")]
        SysMenu,
        [Display(Name = "Shop Menu")]
        ShopMenu
    }

    public enum PictureType
    {
        [Display(Name = "Avatar")]
        Avatar,
        [Display(Name = "Product")]
        Product,
        [Display(Name = "Category")]
        Category
    }

    public enum PictureSize
    {
        Tiny,
        Small,
        Medium,
        Large,
        Auto
    }


    public enum ModelTypeName
    {
        [Display(Name = "User")]
        User,
        [Display(Name = "Product")]
        Product,
        [Display(Name = "Category")]
        Category
    }

    public enum ShopType
    {
        Secured, //Shop đảm bảo
        Normal, //Shop do người dùng tạo
        Unreal  //Shop do quản trị tạo
    }

    public enum AreaType
    {
        City, District, Area    //Area: Hàng xanh, Chợ bến thành v.v...
    }

    /// <summary>
    /// Where lamda operation
    /// </summary>
    public enum BinaryExpressionName
    {
        NotEqual,
        Equal,
        GreaterThan,
        LessThan,
        GreaterThanOrEqual,
        LessThanOrEqual,
        Contains,
        StartsWith,
        EndsWith
    }
}
