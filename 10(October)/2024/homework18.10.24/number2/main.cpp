#include <iostream>
#include <iomanip>
#include <fstream>
#include <conio.h>
#include <string>
#include "phonebook.h"
using namespace std;

int main(){
    ifstream book;
    int sc=0, size=0, nw=0, proverka=0;
    cout<<"Enter phonebook size: ";
    cin>>size;
    cin.ignore(); 
    phoneb users[size];
    string command;
    book.open("phonebook.txt");
    for (int i=0; i<size; i++){
        users[i].filevvod(book, proverka);
    }    
    cout<<"List of commands:"<<endl;
    cout<<"\"fill\" - fill the ghost's place"<<endl;
    cout<<"\"search\" - search by full name"<<endl;
    cout<<"\"print\" - print all list"<<endl;
    cout<<"\"delete\" - delete a person from the list"<<endl;
    cout<<"\"stop\" - stop the programm"<<endl;
    cout<<"(data is saved automatically)"<<endl<<endl;
    while(true){
        cout<<"Enter command -> ";
        getline(cin,command);
        if (command=="fill"){
            proverka=1;
            for (int i=0; i<size; i++){
                users[i].filevvod(book, proverka);
            }
        }else if (command=="search"){
            string person="";
            cout<<"Who are you looking for?"<<endl;
            getline(cin,person);
            for (int i=0; i<size; i++){
                if(person==users[i].getname()){
                    users[i].vivod();
                }
            }
        } else if (command=="print"){
            cout<<endl;
            for (int i=0; i<size; i++){
                users[i].vivod();
            }
        } else if(command=="delete"){
            string person="";
            cout<<"Enter the full name of the person you want to delete: ";
            getline(cin, person);
            int zap=0;
            for (int i=0; i<size; i++){
                if (users[i].getname()==person){
                    zap+=1;
                }
                if (zap!=0 && i+1!=size){
                    users[i]=users[i+1];
                }else if (zap!=0 && i+1==size){
                    users[i].delet();
                }
            }
            zap=0;
        } else if(command=="stop"){
            break;
        } else {
            cout<<"UNKNOWN COMMAND"<<endl;
        }
        ofstream bookin;
        bookin.open("phonebook.txt");
        for (int i=0; i<size; i++){
            users[i].filevivod(bookin);
        }
        bookin.close();
    }
    _getch();
    return 0;
}


    // ofstream out;
    // out.open ("FILENAME",std::ios::trunc | std::ios::binary);
    // out.open("phonebook.txt");
    // while(h==1){
    //     out<<"user "<<users<<" :";
    //     cout<<"write \"break\" if you want to end\nwrite \"enter\" if you want to continue\n";
    //     cin>>command;
    //     cout<<"user "<<users<<" :"<<endl;
    //     if (command=="break"){
    //         break;
    //     } else{
    //         cout<<"";
    //     }
    //     user.vvod();
    //     user.vivod();
    //     users+=1;
    // }
    // out.close();

    // for (int i; i<size; i++){
    //     users[i].vvod();
    // }