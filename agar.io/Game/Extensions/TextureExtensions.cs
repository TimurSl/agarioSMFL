using SFML.Graphics;

namespace SFML_Animation_Practice.Game.Extensions;

public static class TextureExtensions
{
	public static Texture RemoveColor(this Texture texture, Color targetColor)
	{
		byte[] pixels = texture.CopyToImage().Pixels;
			
		uint width = texture.Size.X;
		uint height = texture.Size.Y;

		for (uint y = 0; y < height; y++)
		{
			for (uint x = 0; x < width; x++)
			{
				uint index = (y * width + x) * 4;
				byte red = pixels[index];
				byte green = pixels[index + 1];
				byte blue = pixels[index + 2];
				byte alpha = pixels[index + 3];
					
				if (red == targetColor.R && green == targetColor.G && blue == targetColor.B)
				{
					pixels[index + 3] = 0;
				}
			}
		}
		
		texture.Update(pixels);
		
		return texture;
	}
	
	public static IntRect ToTextureRect(this Texture texture)
	{
		return new IntRect(0, 0, (int)texture.Size.X, (int)texture.Size.Y);
	}
}