#include <stdio.h>

class test{
public:
    test(){
        printf("test::dotest\n");
    }
};

int func(int i){
    printf("func\n");
    return 0;
}

int func1(int i, int j){
    printf("func1\n");
    return 0;
}

int func2(int i, int j, int k){
    printf("func2\n");
    return 0;
}

test t;

int main(){
    func(1);
    func1(1, 2);
    func2(1, 2, 3);
    printf("main\n");
    return -1;
}
