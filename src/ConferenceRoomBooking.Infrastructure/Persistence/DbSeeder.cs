using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Rooms.AnyAsync())
            return;

        var roomA = new Room("Зал А", 50, new Money(2000));
        roomA.AddService("Проєктор", new Money(500));
        roomA.AddService("Wi-Fi", new Money(300));

        var roomB = new Room("Зал B", 100, new Money(3500));
        roomB.AddService("Проєктор", new Money(500));
        roomB.AddService("Wi-Fi", new Money(300));
        roomB.AddService("Звук", new Money(700));

        var roomC = new Room("Зал C", 30, new Money(1500));
        roomC.AddService("Wi-Fi", new Money(300));

        context.Rooms.AddRange(roomA, roomB, roomC);

        await context.SaveChangesAsync();
    }
}