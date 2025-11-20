public static class HomeGroup
{
    public static RouteGroupBuilder MapHomeApi(this RouteGroupBuilder grp)
    {
        grp.MapGet("/", ()=>"default");
        grp.MapGet("/aaa", ()=>"AAAA");
        grp.MapGet("/bbb", ()=>"BBBB");
        return grp;
    }
}