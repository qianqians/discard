/*
 * writebuff.cpp
 *   Created on: 2013-10-5
 *       Author: qianqians
 * writebuff
 */
#include "writebuff.h"

namespace Hemsleya { 
namespace async_net { 

writebuff::writebuff() : _list(0){
}
	
writebuff::~writebuff(){
}

int writebuff::write(char * buf, uint32_t len, boost::function<void(const std::vector<buffstruct> * const buff)> fn){
	boost::shared_lock<boost::shared_mutex> lock(_mutex);

	std::vector<writebuffstruct> * pbuf = tss_buff.get();
	if (pbuf == 0){
		pbuf = _abstract_factory_vector_buffstruct.create_product();
		tss_buff.reset(pbuf);
		buflist * _list_ = 0;
		_list_->buf = pbuf;
		_list_->next = _list.exchange(_list_);
	}

	if (pbuf->size() <= 0 || (pbuf->back().len+len) > pbuf->back().max){
		uint32_t num = (len + 65535)/65536;

		writebuffstruct writebuf;
		writebuf.buff = (char*)_abstract_factory_buff.create_product(num);
		writebuf.max = num*65536;
		pbuf->push_back(writebuf);
	}

	memcpy(pbuf->back().buff+pbuf->back().len, buf, len);

	lock.unlock();

	boost::unique_lock<boost::shared_mutex> unique_lock(_mutex, boost::try_to_lock);
	if (unique_lock.owns_lock()){
		std::vector<buffstruct> outbuf;

		for(buflist * plist = _list.load(); plist != 0; plist = plist->next){
			for(auto buf : *(plist->buf)){
				buffstruct _buf_;
				_buf_.buff = buf.buff;
				_buf_.len = buf.len;

				outbuf.push_back(_buf_);
			}
		}

		fn(&outbuf);
	}

	return 0;
}

void writebuff::freebuff(char * buff, uint32_t buflen){
	_abstract_factory_buff.release_product((_buff*)buff, (buflen+65535)/65536);
}

} //async_net
} //Hemsleya
