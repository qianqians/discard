/*
 * mongoproxy.h
 *
 *  Created on: 2015-3-31
 *      Author: qianqians
 */
#ifndef _mongoproxy_h
#define _mongoproxy_h

#include <mongoc.h>

#include <string>
#include <boost/unordered_map.hpp>

#include "../config/config.h"

namespace Fossilizid{
namespace mongoproxy{

class dbproxy{
public:
	dbproxy(std::string ip, short port, std::string db);
	~dbproxy();

public:
	bool create_index(std::string collection_name, std::string keys);
	
	bool drop_index(std::string collection, std::string index_name);

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > find_indexes(std::string  collection);

	int64_t count(std::string collection_name, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, int skip, int limit);
	
	bool insert(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document);
	
	bool save(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document);
	
	bool update(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update);

	bool remove(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query);

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > find(std::string collection, int skip, int limit, int batch_size, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, std::vector<std::string> fields);

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > find_and_modify(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort, boost::shared_ptr<boost::unordered_map<std::string, boost::any> >  update, std::vector<std::string>  fields, bool _remove, bool upsert, bool _new);

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > validate(std::string collection);

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > stats(std::string collection);

private:
	mongoc_collection_t * get_collection(std::string collection_name);

private:
	mongoc_client_t * _client;
	mongoc_database_t * _db;
	boost::unordered_map<std::string, mongoc_collection_t *> collectiondict;

};

} /* namespace mongoproxy */
} /* namespace Fossilizid */

#endif //_log_h