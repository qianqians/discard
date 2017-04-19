/*
 *  no title
 *  negative value
 */
#include <pin.H>
#include <string>
#include <iostream>
#include <fstream>

ofstream log;

VOID funcbefore(CHAR * rtnname, INT32 argv0, INT32 argv1, INT32 argv2){
    log << rtnname << std::endl;
    log << "argv0:" << argv0 << std::endl;
log << "argv1:" << argv1 << std::endl;
log << "argv2:" << argv2 << std::endl;
}

VOID funcafter(CHAR * rtnname, int ret){
    std::cout << rtnname << std::endl;
}

VOID rtnfunc(RTN rtn, VOID *v){
    if (RTN_Name(rtn) != "_Z5func2iii"){
        return ;
    }
    RTN_Open(rtn);
    RTN_InsertCall(rtn, IPOINT_BEFORE, (AFUNPTR)funcbefore,
                   IARG_ADDRINT, RTN_Name(rtn).c_str(), 
                   IARG_FUNCARG_CALLSITE_VALUE, 0,
IARG_FUNCARG_CALLSITE_VALUE, 1,
IARG_FUNCARG_CALLSITE_VALUE, 2,
                   IARG_END);
    
    RTN_InsertCall(rtn, IPOINT_AFTER, (AFUNPTR)funcafter, 
                   IARG_ADDRINT, RTN_Name(rtn).c_str(), 
                   IARG_RETURN_IP,
                   IARG_END);
    RTN_Close(rtn);
}

VOID fini(INT32 code, VOID *v){
    /*
     *  end program
     */
}

int main(int argc, char * argv[]){
    log.open("log.log", ios_base::out);    

    if (PIN_Init(argc, argv)){
        /*
         *  init error
         */
	return 0;
    }

    PIN_InitSymbols();

    RTN_AddInstrumentFunction(rtnfunc, 0);

    PIN_AddFiniFunction(fini, 0);

    PIN_StartProgram();

    return 0;
}
