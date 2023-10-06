using MiNET.Worlds;

namespace MiNET.Utils;

public static class GamemodeExtensions
{
	public static bool AllowsFlying(this GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Spectator:
			case GameMode.Creative:
				return true;
			default:
				return false;
		}
	}
	
	public static bool HasCollision(this GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Spectator:
				return false;
			default:
				return true;
		}
	}
	
	public static bool AllowsTakingDamage(this GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Creative:
			case GameMode.Spectator:
				return false;
			default:
				return true;
		}
	}
	
	public static bool HasCreativeInventory(this GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Creative:
				return true;
			default:
				return false;
		}
	}
	
	public static bool AllowsEditing(this GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Adventure:
			case GameMode.Spectator:
				return false;
			default:
				return true;
		}
	}
	
	public static bool AllowsInteraction(this GameMode gameMode)
	{
		switch (gameMode)
		{
			case GameMode.Spectator:
				return false;
			default:
				return true;
		}
	}
}