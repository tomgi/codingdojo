//this example shows how to set up a palette and use 8x8 HW sprites
//also shows how fast (or slow) C code is

#include "neslib.h"


//general purpose vars

static unsigned char j;
static unsigned char spr;


typedef struct zz {
	unsigned char x;
	unsigned char y;
	unsigned char dx;
	unsigned char dy;

	void (*Update)(struct zz*);

} Ball;

static Ball ball;

void updatePtr(Ball* this)
{
	//move the this

	this->x+=this->dx;
	this->y+=this->dy;

	//bounce the this off the edges

	if(this->x>=(256-8) || this->x<8) this->dx=-this->dx;
	if(this->y>=(240-8) || this->y<8) this->dy=-this->dy;
}

//palette for balls, there are four sets for different ball colors

const unsigned char palSprites[16]={
	0x0f,0x17,0x27,0x37,
	0x0f,0x11,0x21,0x31,
	0x0f,0x15,0x25,0x35,
	0x0f,0x19,0x29,0x39
};


static int dupa = 0;

void main(void)
{
	pal_spr(palSprites);//set palette for sprites

	ppu_on_all();//enable rendering

	ball.Update = &updatePtr;

	//starting coordinates

	ball.x=rand8();
	ball.y=rand8();

	//direction bits

	j=rand8();

	//horizontal speed -3..-3, excluding 0
	spr=3;
	ball.dx=j&1?-spr:spr;

	//vertical speed

	spr=3;
	ball.dy=j&2?-spr:spr;

	
	//now the main loop

	while(1)
	{
		ppu_wait_frame();//wait for next TV frame

		ball.Update(&ball);
		//set a sprite for current ball

		dupa = oam_spr(ball.x,ball.y,0x40,0,dupa);//0x40 is tile number, i&3 is palette

	}
}