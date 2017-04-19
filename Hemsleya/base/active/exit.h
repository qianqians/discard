/*
 * exit.h
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */

#ifndef EXIT_H_
#define EXIT_H_

#include <boost/atomic.hpp>

namespace Hemsleya {
namespace active {

extern boost::atomic_bool is_run;

} /* namespace active */
} /* namespace Hemsleya */
#endif /* EXIT_H_ */
