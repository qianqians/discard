/*
 * log.h
 *
 *  Created on: 2015-3-25
 *      Author: qianqians
 */
#ifndef _log_h
#define _log_h

#include <string>
#include <sstream>

#include <boost/bind.hpp>
#include <boost/make_shared.hpp>
#include <boost/log/trivial.hpp>
#include <boost/log/trivial.hpp>  
#include <boost/log/expressions.hpp>  
#include <boost/log/utility/setup/file.hpp> 

#include <pool/mempool.h>

namespace Fossilizid{
namespace log{

class log{
public:
	static boost::shared_ptr<log> createinstance(std::string & module){
		log * _log = (log*)pool::mempool::allocator(sizeof(log));
		new (_log) log(module);
		return boost::shared_ptr<log>(_log, boost::bind(log::releaselog, _1));
	}

public:
	void setfile(std::string & file);

	void trace(char * format, ...){
		setlog();

		char buf[4096];

		va_list ap;
		sprintf_s(buf, (std::string("%s:%s(%d)-<%s>: ") + format).c_str(), _module.c_str(), __FILE__, __LINE__, __FUNCTION__, ap);
		va_end (ap);

		BOOST_LOG_TRIVIAL(trace) << buf;
	}
	
	void debug(char * format, ...){
#ifdef _DEBUG
		setlog();

		char buf[4096];

		va_list ap;
		sprintf_s(buf, (std::string("%s:%s(%d)-<%s>: ") + format).c_str(), _module.c_str(), __FILE__, __LINE__, __FUNCTION__, ap);
		va_end (ap);

		BOOST_LOG_TRIVIAL(debug) << buf;
#endif
	}

	void info(char * format, ...){
		setlog();

		char buf[4096];

		va_list ap;
		sprintf_s(buf, (std::string("%s:%s(%d)-<%s>: ") + format).c_str(), _module.c_str(), __FILE__, __LINE__, __FUNCTION__, ap);
		va_end (ap);

		BOOST_LOG_TRIVIAL(info) << buf;
	}

	void warning(char * format, ...){
		setlog();

		char buf[4096];

		va_list ap;
		sprintf_s(buf, (std::string("%s:%s(%d)-<%s>: ") + format).c_str(), _module.c_str(), __FILE__, __LINE__, __FUNCTION__, ap);
		va_end (ap);

		BOOST_LOG_TRIVIAL(warning) << buf;
	}

	void error(char * format, ...){
		setlog();

		char buf[4096];

		va_list ap;
		sprintf_s(buf, (std::string("%s:%s(%d)-<%s>: ") + format).c_str(), _module.c_str(), __FILE__, __LINE__, __FUNCTION__, ap);
		va_end (ap);

		BOOST_LOG_TRIVIAL(error) << buf;
	}

	void fatal(char * format, ...){
		setlog();

		char buf[4096];

		va_list ap;
		sprintf_s(buf, (std::string("%s:%s(%d)-<%s>: ") + format).c_str(), _module.c_str(), __FILE__, __LINE__, __FUNCTION__, ap);
		va_end (ap);

		BOOST_LOG_TRIVIAL(fatal) << buf;
	}

private:
	static void releaselog(log * _log){
		_log->~log();

	}

	log(std::string & module){
		_module = module;
	}

	~log(){
	}

	void setlog();

	std::string _module;

	std::string logfile;

};

} /* namespace log */
} /* namespace Fossilizid */

#endif //_log_h