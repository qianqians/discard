/*
 * mongoproxy.cpp
 *
 *  Created on: 2015-3-31
 *      Author: qianqians
 */
#include "mongoproxy.h"

#include <boost/make_shared.hpp>
#include <JsonParser.h>

namespace Fossilizid{
namespace mongoproxy{

dbproxy::dbproxy(std::string ip, short port, std::string db){
	mongoc_init();
	
	mongoc_uri_t * _uri = mongoc_uri_new_for_host_port(ip.c_str(), port);
	_client = mongoc_client_new(mongoc_uri_get_string(_uri));
	mongoc_uri_destroy(_uri);

	_db = mongoc_client_get_database(_client, db.c_str());
}

dbproxy::~dbproxy(){
	for(auto it : collectiondict){
		mongoc_collection_destroy(it.second);
	}
	mongoc_database_destroy(_db);
	mongoc_client_destroy(_client);
	mongoc_cleanup();
}

mongoc_collection_t * dbproxy::get_collection(std::string collection_name){
	auto it = collectiondict.find(collection_name);
	if (it != collectiondict.end()){
		return it->second;
	}

	auto c = mongoc_database_get_collection(_db, collection_name.c_str());
	collectiondict.insert(std::make_pair(collection_name, c));

	return c;
}


bool dbproxy::create_index(std::string collection_name, std::string keys){
	mongoc_collection_t * _c = get_collection(collection_name);

	bson_t bkeys;
	mongoc_index_opt_t opt;
	bson_error_t error;

	bson_init(&bkeys);
	mongoc_index_opt_init(&opt);
	bson_append_int32(&bkeys, keys.c_str(), -1, 1);

	return mongoc_collection_create_index(_c, &bkeys, &opt, &error);
}
	
bool dbproxy::drop_index (std::string collection, std::string index_name){
	mongoc_collection_t * _c = get_collection(collection);
	
	bson_error_t error;
	return mongoc_collection_drop_index(_c, index_name.c_str(), &error);
}

std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > dbproxy::find_indexes(std::string  collection){
	mongoc_collection_t * _c = get_collection(collection);

	bson_error_t error;
	auto c = mongoc_collection_find_indexes(_c, &error);

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;

	const bson_t *doc;
	while (mongoc_cursor_next(c, &doc)){
		auto json = bson_as_json(doc, 0);
		Fossilizid::JsonParse::JsonObject o;
		Fossilizid::JsonParse::unpacker(o, json);
		
		ret.push_back(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(o));
	}

	return ret;
}

int64_t dbproxy::count(std::string collection_name, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, int skip, int limit){
	mongoc_collection_t * _c = get_collection(collection_name);

	bson_error_t error;
	bson_t bquery;

	auto json = Fossilizid::JsonParse::packer(query);
	bson_init_from_json(&bquery, json.c_str(), json.length(), &error);
	
	return mongoc_collection_count(_c, MONGOC_QUERY_NONE, &bquery, skip, limit, nullptr, &error);
}
	
bool dbproxy::insert(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document){
	mongoc_collection_t * _c = get_collection(collection);
	
	bson_error_t error;
	bson_t bdoc;
	
	auto json = Fossilizid::JsonParse::packer(document);
	bson_init_from_json(&bdoc, json.c_str(), json.length(), &error);
	
	return mongoc_collection_insert(_c, MONGOC_INSERT_NONE, &bdoc, nullptr, &error);
}
	
bool dbproxy::save(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document){
	mongoc_collection_t * _c = get_collection(collection);
	
	bson_error_t error;
	bson_t bdoc;

	auto json = Fossilizid::JsonParse::packer(document);
	bson_init_from_json(&bdoc, json.c_str(), json.length(), &error);

	return mongoc_collection_save(_c, &bdoc, nullptr, &error);
}
	
bool dbproxy::update(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update){
	mongoc_collection_t * _c = get_collection(collection);

	bson_error_t error;

	bson_t bquery;
	auto qjson = Fossilizid::JsonParse::packer(query);
	bson_init_from_json(&bquery, qjson.c_str(), qjson.length(), &error);

	bson_t bupdate;
	auto ujson = Fossilizid::JsonParse::packer(update);
	bson_init_from_json(&bupdate, ujson.c_str(), ujson.length(), &error);

	return mongoc_collection_update(_c, MONGOC_UPDATE_NONE, &bquery, &bupdate, nullptr, &error);
}

bool dbproxy::remove(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query){
	mongoc_collection_t * _c = get_collection(collection);

	bson_error_t error;

	bson_t bquery;
	auto qjson = Fossilizid::JsonParse::packer(query);
	bson_init_from_json(&bquery, qjson.c_str(), qjson.length(), &error);

	return mongoc_collection_remove(_c, MONGOC_REMOVE_NONE, &bquery, nullptr, &error);
}

std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > dbproxy::find(std::string collection, int skip, int limit, int batch_size, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, std::vector<std::string> fields){
	mongoc_collection_t * _c = get_collection(collection);

	bson_error_t error;

	bson_t bquery;
	auto qjson = Fossilizid::JsonParse::packer(query);
	bson_init_from_json(&bquery, qjson.c_str(), qjson.length(), &error);

	bson_t bfields;
	for (int i = 0; i < fields.size(); i++){
		std::stringstream ss;
		ss << i;

		BSON_APPEND_UTF8(&bfields, ss.str().c_str(), fields[i].c_str());
	}

	auto c = mongoc_collection_find(_c, MONGOC_QUERY_NONE, skip, limit, batch_size, &bquery, &bfields, 0);

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;

	const bson_t *doc;
	while (mongoc_cursor_next(c, &doc)){
		auto json = bson_as_json(doc, 0);
		Fossilizid::JsonParse::JsonObject o;
		Fossilizid::JsonParse::unpacker(o, json);

		ret.push_back(Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(o));
	}

	return ret;
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > dbproxy::find_and_modify(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort, boost::shared_ptr<boost::unordered_map<std::string, boost::any> >  update, std::vector<std::string>  fields, bool _remove, bool upsert, bool _new){
	mongoc_collection_t * _c = get_collection(collection);
	
	bson_error_t berror;

	bson_t bquery;
	auto qjson = Fossilizid::JsonParse::packer(query);
	bson_init_from_json(&bquery, qjson.c_str(), qjson.length(), &berror);
	
	bson_t bsort;
	auto bjson = Fossilizid::JsonParse::packer(sort);
	bson_init_from_json(&bquery, bjson.c_str(), bjson.length(), &berror);

	bson_t bupdate;
	auto ujson = Fossilizid::JsonParse::packer(query);
	bson_init_from_json(&bquery, ujson.c_str(), ujson.length(), &berror);
	
	bson_t bfields;
	for (int i = 0; i < fields.size(); i++){
		std::stringstream ss;
		ss << i;

		BSON_APPEND_UTF8(&bfields, ss.str().c_str(), fields[i].c_str());
	}

	bson_t breply;

	if (mongoc_collection_find_and_modify(_c, &bquery, &bsort, &bupdate, &bfields, _remove, upsert, _new, &breply, &berror)){
		auto rjson = bson_as_json(&breply, nullptr);

		Fossilizid::JsonParse::JsonObject ret;
		Fossilizid::JsonParse::unpacker(ret, rjson);

		return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(ret);
	}

	return nullptr;
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > dbproxy::validate(std::string collection){
	mongoc_collection_t * _c = get_collection(collection);

	bson_t opts;
	bson_init(&opts);
	BSON_APPEND_BOOL(&opts, "full", true);

	bson_t reply;
	bson_error_t error;

	if (mongoc_collection_validate(_c, &opts, &reply, &error)){
		auto rjson = bson_as_json(&reply, nullptr);

		Fossilizid::JsonParse::JsonObject ret;
		Fossilizid::JsonParse::unpacker(ret, rjson);

		return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(ret);
	}

	return nullptr;
}
	
boost::shared_ptr<boost::unordered_map<std::string, boost::any> > dbproxy::stats(std::string collection){
	mongoc_collection_t * _c = get_collection(collection);

	bson_t opts;
	bson_init(&opts);
	BSON_APPEND_BOOL(&opts, "full", true);

	bson_t reply;
	bson_error_t error;

	if (mongoc_collection_stats(_c, &opts, &reply, &error)){
		auto rjson = bson_as_json(&reply, nullptr);

		Fossilizid::JsonParse::JsonObject ret;
		Fossilizid::JsonParse::unpacker(ret, rjson);

		return Fossilizid::JsonParse::JsonCast<Fossilizid::JsonParse::JsonTable>(ret);
	}

	return nullptr;
}


} /* namespace mongoproxy */
} /* namespace Fossilizid */

