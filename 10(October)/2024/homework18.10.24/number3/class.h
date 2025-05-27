#include <iostream>
#include <cstring>
using namespace std;

class String{
private:
    string* stroka;
    int size;
    static int scet;
public:
    String() 
        { stroka = new string[80] {} , size=80 , scet+=1; }

    String(int sizeP)
        { stroka = new string[sizeP] {""}, size = sizeP, scet+=1; }

    String(int sizeP, string strokaP)
        { stroka = new string[sizeP] {strokaP},size = sizeP, scet+=1;}
    
    ~String(){
        delete[] stroka;
        scet-=1;
    }


    void random(){
        for (int i=0; i<size; i++){
            stroka[i]=(char)rand()%205+50;
        }
    }


    void print(){
        for (int i=0; i<size; i++){
            cout<<stroka[i];
        }
    }

    static int getscet(){
        return scet;
    }
};