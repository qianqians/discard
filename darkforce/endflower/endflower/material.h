/*
* qianqians
* 2015-10-28
*/
#ifndef _material_h_
#define _material_h_

#include <boost/array.hpp>
#include <list>

class material {
public:
	enum materialtype {
		selflight = 0,
		reflect = 1,
		anacampsis = 2,
	};

	struct _material {
		struct lightsrc {
			enum {
				ambient = 0,
				directional = 1,
			} _lighttype;

			float _color[3];
			float brightness;
			float direction;
		};

		struct reflect {
			float _color[3];
			float absorb;
		};

		struct anacampsis {
			float _color[3];
			float absorb;
			float refractivity;
		};
	
		materialtype _materialtype;
		union {
			lightsrc _lightsrcmate;
			reflect _reflectmate;
			anacampsis _anacampsismate;
		};
	};

	std::list<_material> element;

};

#endif