#pragma once

#include "stdafx.h"

using namespace System;

namespace EotuCore {

	public ref class GroupModel
	{
	private:
		int id;
		String^ name;

	public:
		GroupModel(void){
			id = 0;
			name = String::Empty;
		}

		property int GroupId
		{
			int get() {
				return id;
			}

			void set(int value) {
				if (value != id)
				{
					id = value;
				}
			}
		}

		property String^ GroupName
		{
			String^ get() {
				return name;
			}

			void set(String^ value) {
				if (value != name)
				{
					name = value;
				}
			}
		}
	};
}

