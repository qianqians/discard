function centerproxy(ch){
    this.is_reg_center_sucess = false;
    this.center = new center_caller(ch);

    this.reg_server = function( ip,  port,  uuid ){
        this.center.reg_server("gate", ip, port, uuid);
	}
}
