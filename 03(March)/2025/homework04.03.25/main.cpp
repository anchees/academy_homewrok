#include <iostream>
using namespace std;

class Image
{
private:
    const int Width;
    const int Length;

    class Pixel
    {
    private:
        int Red;
        int Green;
        int Blue;
    public:
        Pixel(int r = 0, int g = 0, int b = 0)
            : Red(r), Green(g), Blue(b) {}

        void SetColor(int r, int g, int b) {
            Red = r;
            Green = g;
            Blue = b;
        }

        void Print() const {
            cout << "(" << Red << "," << Green << "," << Blue << ")";
        }
    };

    Pixel** Picture;

public:
    Image(int W, int L) : Width(W), Length(L)
    {
        Picture = new Pixel*[Width];
        for (int i = 0; i < Width; i++) {
            Picture[i] = new Pixel[Length];
        }
    }

    ~Image() {
        for (int i = 0; i < Width; i++) {
            delete[] Picture[i];
        }
        delete[] Picture;
    }

    void SetPixel(int x, int y, int r, int g, int b) {
        if (x >= 0 && x < Width && y >= 0 && y < Length) {
            Picture[x][y].SetColor(r, g, b);
        }
    }

    void PrintImage() const {
        for (int y = 0; y < Length; y++) {
            for (int x = 0; x < Width; x++) {
                Picture[x][y].Print();
                cout << " ";
            }
            cout << endl;
        }
    }
};


int main() {
    Image img(3, 2);  
    img.SetPixel(0, 0, 255, 0, 0);  
    img.SetPixel(1, 0, 0, 255, 0);  
    img.SetPixel(2, 0, 0, 0, 255);  

    img.SetPixel(0, 1, 255, 255, 0); 
    img.SetPixel(1, 1, 0, 255, 255); 
    img.SetPixel(2, 1, 255, 0, 255); 

    img.PrintImage();

    return 0;
}
