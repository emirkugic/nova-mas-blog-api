
using System.Text.Json.Serialization;

namespace nova_mas_blog_api.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BlogCategory
    {
        Trending,
        StreetStyle,
        Couture,
        Vintage,
        CasualWear,
        FormalWear,
        Seasonal,
        Accessories,
        DIY,
        BeautyTips
    }
}