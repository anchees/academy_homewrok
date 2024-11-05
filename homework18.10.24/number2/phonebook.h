#include <iostream>
#include <cstring>
using namespace std;

class phoneb{
private:
    char *name;
    string nname;
    string homep;
    string workp;
    string mobilep;
    string moreinfo;
    static int scet;
public:
    phoneb() : name(nullptr), homep(""), workp(""), mobilep(""), 
    moreinfo("") {}


    phoneb(const char* nname, const string& nhomep, const string& nworkp,
    const string& nmobilep, const string& nmoreinfo) :
    homep(nhomep), workp(nworkp), mobilep(nmobilep),
    moreinfo(nmoreinfo) {}


    ~phoneb(){
        delete[] name;
        
    }

    inline const char* getname() { return name; }
    inline const string& gethomep() { return homep; }
    inline string& getworkp() { return workp; }
    inline string& getmobilep() { return mobilep; }
    inline string& getmoreinfo() { return moreinfo; }
    // void vvod();
    void filevvod(ifstream &book, int proverka);
    void filevivod(ofstream &bookin);
    void vivod();
    void delet();
};