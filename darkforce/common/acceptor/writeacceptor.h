/*
 * writeacceptor.h
 *
 *  Created on: 2015-3-26
 *      Author: qianqians
 */
#ifndef _writeacceptor_h
#define _writeacceptor_h

#include <string>

#include <boost/shared_ptr.hpp>

#include "../achieve/achieve.h"
#include "../fitle/fitle.h"
#include "../config/config.h"


namespace Fossilizid{
namespace acceptor{
	
class writeacceptor{
public:
	writeacceptor(std::string ip, short port, boost::shared_ptr<std::vector<std::pair<std::string, short> > > _set, boost::shared_ptr<achieve::channelservice> _chservice, boost::shared_ptr<achieve::sessioncontainer> _sc);
	~writeacceptor();

	void add_write_addr(std::string ip, short port);

private:
	boost::shared_ptr<std::vector<std::pair<std::string, short> > > set;

	boost::shared_ptr<achieve::acceptor> acp;
	boost::shared_ptr<fitle::fitle<std::pair<std::string, short> > > epfitle;

};

} /* namespace acceptor */
} /* namespace Fossilizid */

#endif //_writeacceptor_h
