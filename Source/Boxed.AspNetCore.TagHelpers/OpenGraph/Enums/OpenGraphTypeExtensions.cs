namespace Boxed.AspNetCore.TagHelpers.OpenGraph
{
    /// <summary>
    /// <see cref="OpenGraphType"/> extension methods.
    /// </summary>
    internal static class OpenGraphTypeExtensions
    {
        /// <summary>
        /// Returns the lower-case <see cref="string"/> representation of the <see cref="OpenGraphType"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The lower-case <see cref="string"/> representation of the <see cref="OpenGraphType"/>.</returns>
        public static string ToLowercaseString(this OpenGraphType type) =>
            type switch
            {
                OpenGraphType.Article => "article",
                OpenGraphType.Book => "book",
                OpenGraphType.BooksAuthor => "books.author",
                OpenGraphType.BooksBook => "books.book",
                OpenGraphType.BooksGenre => "books.genre",
                OpenGraphType.Business => "business.business",
                OpenGraphType.FitnessCourse => "fitness.course",
                OpenGraphType.GameAchievement => "game.achievement",
                OpenGraphType.MusicAlbum => "music.album",
                OpenGraphType.MusicPlaylist => "music.playlist",
                OpenGraphType.MusicRadioStation => "music.radio_station",
                OpenGraphType.MusicSong => "music.song",
                OpenGraphType.Place => "place",
                OpenGraphType.Product => "product",
                OpenGraphType.ProductGroup => "product.group",
                OpenGraphType.ProductItem => "product.item",
                OpenGraphType.Profile => "profile",
                OpenGraphType.RestaurantMenu => "restaurant.menu",
                OpenGraphType.RestaurantMenuItem => "restaurant.menu_item",
                OpenGraphType.RestaurantMenuSection => "restaurant.menu_section",
                OpenGraphType.Restaurant => "restaurant.restaurant",
                OpenGraphType.VideoEpisode => "video.episode",
                OpenGraphType.VideoMovie => "video.movie",
                OpenGraphType.VideoOther => "video.other",
                OpenGraphType.VideoTvShow => "video.tv_show",
                OpenGraphType.Website => "website",
                _ => "website",
            };
    }
}
