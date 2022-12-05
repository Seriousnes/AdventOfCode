namespace AdventOfCode.API;

public static class Session
{
    /// <summary>
    /// This is your session token which you can obtain from your browser dev tools
    /// </summary>
    public static string Cookie = "53616c7465645f5fdadf9349ca8db039e9846a7239821d54e54593ab96a7042125997c17da0c630b54719405ba9cd75be34ab5b24ea8b880a5bd677961750540";
    /// <summary>
    /// Contact info such as email address or github that will be included in the UserAgent header
    /// </summary>
    /// <remarks>
    /// Example: github.com/topaz/name-of-tool by yourname@example.com
    /// </remarks>
    public static string ContactInfo = "https://github.com/Seriousnes/AdventOfCode by seansmith7@gmail.com";
}