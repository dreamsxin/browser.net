/*
 *  Copyright (c) 2014, Oculus VR, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 *
 */

#if   defined(_WIN32)
#include <conio.h> /* getche() */
#ifdef _MSC_VER
#define getche() _getche()
#endif
#else
#include <termios.h>
#include <stdio.h>
#include <unistd.h>
char getche();
#endif 
