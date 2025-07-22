# Домашняя работа до 04.03.25
## Задание
Урок 11 стр. с 1 по 19. Реализовать объемлющий(внешний) класс Image и вложенный в него класс Pixel

Образец:
``` cpp
class Image
{
private:
	const int Weidth;
	const int Lenght;

	class Pixel
	{
		int Red;
		int Green;
		int Blue;
	};

	Pixel** Picture;
public:
	Image(int W,int L):Weidth{W},Lenght{L}
	{
		Picture = new Pixel*[Weidth]{};
		for (int i = 0; i < Weidth; i++)
		{
			Picture[i] = new Pixel[Lenght];
		}
	}
};
```