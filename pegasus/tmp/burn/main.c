#include "neslib.h"
#include "test.h"

#define NTADR(x,y) ((0x2000|((y)<<5)|x))
#define MSB(x) (((x)>>8))
#define LSB(x) (((x)&0xff))

const unsigned char palSprites[16]={
	0x0f,0x17,0x27,0x37,
	0x0f,0x11,0x21,0x31,
	0x0f,0x15,0x25,0x35,
	0x0f,0x19,0x29,0x39
};

static unsigned char vram_buffer[896]; // 896:32*28
static unsigned char spr;

void put_str(unsigned int adr,const char *str)
{
	vram_adr(adr);

	while(1)
	{
		if(!*str) break;
		vram_put((*str++)-0x20); //-0x20 because ASCII code 0x20 is placed in tile 0 of the CHR
	}
}

void screen_fade_out(interval){
	unsigned char i=0x05;
	while(1){
		//ppu_waitnmi();//wait for next TV frame
		i--;
		pal_bright(i);
		delay(interval);
		if (i==0){
			break;
		}
	}
}

void screen_fade_in(){
	unsigned char i=0x00;
	while(1){
		//ppu_waitnmi();//wait for next TV frame
		i++;
		pal_bright(i);
		delay(5);
		if (i==4){
			break;
			//goto mystart;
		}
	}
}

void init_screen(){
	memfill(vram_buffer, 0, 896); // 896:32*28
	vram_write((unsigned char *)vram_buffer, NTADR(0, 1), 32 * 28);
}

void init(){
	pal_spr(palSprites);//set palette for sprites
	pal_col(1,0x30);//set while color
}

int is_pressed(unsigned char pad){
	unsigned char i;
	i=pad_poll(0);
	return i&pad;
}

typedef struct {
	int x;
	int y;
	unsigned char pattern[];
} sprite_schema;

#define SFX_CHANGE 0
#define SFX_ATTACK 1
#define SFX_FINISH 2
static unsigned char frame;
static unsigned char color_flag;
static unsigned char speed;
static unsigned char score;
static unsigned char time;
static unsigned char is_attacking;
static unsigned char stage;
static unsigned char delta;
static sprite_schema block={0, 0, {0, 0, 0x50, 0,128}};
static sprite_schema wave={0, 0, {0, 0, 0x51, 0,128}};
static sprite_schema mountain={0, 0, {0, 0, 0x52, 0,128}};
static sprite_schema ruby={0, 0, {0, 0, 0x53, 0,128}};
static sprite_schema star={0, 0, {0, 0, 0x54, 0,8, 0, 0x55, 0,0, 8, 0x56, 0,8, 8, 0x57, 0,128}};
static sprite_schema player={0, 0, {0, 0, 0x58, 0,8, 0, 0x59, 0,0, 8, 0x5a, 0,8, 8, 0x5b, 0,128}};
static sprite_schema boss={0, 0, {0, 0, 0x5c, 0,8, 0, 0x5d, 0,16, 0, 0x5e, 0,0, 8, 0x5f, 0,8, 8, 0x60, 0,16, 8, 0x61, 0,128}};
const unsigned char screen_title_color_18[6]={0x01,0xaa,0x01,0x1f,0x01,0x00};
const unsigned char screen_title_color_19[6]={0x01,0xff,0x01,0x0f,0x01,0x00};
const unsigned char screen_title[422]={0x01,0x00,0x01,0x01,0x50,0x01,0x02,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x01,0x02,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x01,0x02,0x50,0x00,0x01,0x06,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x01,0x01,0x00,0x50,0x01,0x01,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x01,0x05,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x01,0x04,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x01,0x05,0x50,0x01,0x02,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x01,0x02,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x01,0x04,0x50,0x00,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x50,0x01,0x04,0x00,0x01,0x04,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x01,0x04,0x50,0x00,0x01,0x01,0x50,0x00,0x01,0x01,0x50,0x00,0x01,0x01,0x50,0x01,0x02,0x00,0x01,0x01,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x50,0x00,0x01,0x02,0x50,0x00,0x01,0x24,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x02,0x53,0x00,0x01,0x02,0x53,0x01,0x02,0x00,0x01,0x02,0x53,0x01,0x02,0x00,0x01,0x01,0x53,0x01,0x01,0x00,0x01,0x04,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x01,0x53,0x00,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x01,0x53,0x00,0x53,0x00,0x01,0x04,0x53,0x01,0x01,0x00,0x01,0x04,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x01,0x53,0x00,0x53,0x00,0x01,0x02,0x53,0x00,0x53,0x00,0x53,0x00,0x01,0x01,0x53,0x01,0x03,0x00,0x01,0x01,0x53,0x01,0x01,0x00,0x01,0x05,0x53,0x00,0x53,0x00,0x53,0x00,0x01,0x01,0x53,0x01,0x04,0x00,0x53,0x01,0x01,0x00,0x01,0x06,0x53,0x00,0x53,0x01,0x01,0x00,0x01,0x05,0x53,0x01,0x01,0x00,0x53,0x01,0x01,0x00,0x01,0x01,0x53,0x00,0x01,0x02,0x53,0x00,0x53,0x00,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x02,0x53,0x00,0x01,0x08,0x53,0x01,0x01,0x00,0x53,0x01,0x01,0x00,0x01,0x01,0x53,0x00,0x01,0x02,0x53,0x00,0x53,0x00,0x01,0x01,0x53,0x00,0x01,0x01,0x53,0x01,0x02,0x00,0x01,0x01,0x53,0x01,0x01,0x00,0x01,0x22,0x01,0x00};
extern const unsigned char music_sarabande[];
extern const unsigned char music_battle[];
static unsigned char game_nmi_list[24*3];
const unsigned char game_nmi_init[24*3]={
MSB(NTADR(2,2)),LSB(NTADR(2,2)),0,
MSB(NTADR(3,2)),LSB(NTADR(3,2)),0,
MSB(NTADR(4,2)),LSB(NTADR(4,2)),0,
MSB(NTADR(5,2)),LSB(NTADR(5,2)),0,
MSB(NTADR(6,2)),LSB(NTADR(6,2)),0,
MSB(NTADR(7,2)),LSB(NTADR(7,2)),0,
MSB(NTADR(8,2)),LSB(NTADR(8,2)),0,
MSB(NTADR(9,2)),LSB(NTADR(9,2)),0,
MSB(NTADR(10,2)),LSB(NTADR(10,2)),0,
MSB(NTADR(11,2)),LSB(NTADR(11,2)),0,
MSB(NTADR(12,2)),LSB(NTADR(12,2)),0,
MSB(NTADR(13,2)),LSB(NTADR(13,2)),0,
MSB(NTADR(14,2)),LSB(NTADR(14,2)),0,
MSB(NTADR(15,2)),LSB(NTADR(15,2)),0,
MSB(NTADR(16,2)),LSB(NTADR(16,2)),0,
MSB(NTADR(17,2)),LSB(NTADR(17,2)),0,
MSB(NTADR(18,2)),LSB(NTADR(18,2)),0,
MSB(NTADR(19,2)),LSB(NTADR(19,2)),0,
MSB(NTADR(20,2)),LSB(NTADR(20,2)),0,
MSB(NTADR(21,2)),LSB(NTADR(21,2)),0,
MSB(NTADR(22,2)),LSB(NTADR(22,2)),0,
MSB(NTADR(23,2)),LSB(NTADR(23,2)),0,
MSB(NTADR(24,2)),LSB(NTADR(24,2)),0,
MSB(NTADR(25,2)),LSB(NTADR(25,2)),0
};

void sprite(sprite_schema *data){
	spr=oam_meta_spr(data->x,data->y,spr,data->pattern);
}

void main(void)
{
init();
frame=0;
color_flag=0;
speed=0x3;
score=0;
time=0xf;
is_attacking=0;
stage=0x1;
delta=0;

//title label starts
title:
pal_col(0,0x38);
pal_col(5,0x22);
pal_col(6,0x22);
pal_col(7,0x22);
pal_col(9,0x26);
pal_col(10,0x16);
pal_col(11,0x16);
unrle_vram(screen_title_color_18,0x23c0);
pal_col(13,0x3d);
pal_col(14,0x2e);
pal_col(15,0x2e);
unrle_vram(screen_title_color_19,0x23d0);
unrle_vram(screen_title,0x2020);
put_str(NTADR(10,20),"PRESS START");
music_play(music_sarabande);
ppu_on_all();
while(1){
ppu_waitnmi(); //wait for next TV frame
frame+=1;
if ((frame%30)==1){star.x=rand8();
star.y=rand8()%50+50;
if (color_flag%2){pal_col(1,0x20);} else {pal_col(1,0x16); } 
color_flag+=1;} 
sprite(&star);
if (is_pressed(PAD_START)){sfx_play(SFX_CHANGE,0);
music_stop();
frame=0;
player.y=220;
screen_fade_out(5);
delay(100);
ppu_off();set_vram_update(0,0);vram_adr(0x2000);vram_fill(0,1024);goto prep_1st;} 
}
ppu_on_all();
while(1);

//prep_1st label starts
prep_1st:
pal_col(0,0x2e);
pal_col(1,0x20);
put_str(NTADR(12,13),"1ST STAGE");
put_str(NTADR(12,16),"OUTBREAK");
ppu_on_all();
screen_fade_in();
delay(600);
screen_fade_out(5);
ppu_off();
set_vram_update(0,0);
vram_adr(0x2000);
vram_fill(0,1024);
goto game;
ppu_on_all();
while(1);

//prep_2nd label starts
prep_2nd:
pal_col(0,0x11);
pal_col(1,0x20);
put_str(NTADR(12,13),"2ND STAGE");
put_str(NTADR(11,16),"BROKEN TIME");
ppu_on_all();
screen_fade_in();
delay(600);
screen_fade_out(5);
ppu_off();
set_vram_update(0,0);
vram_adr(0x2000);
vram_fill(0,1024);
goto game;
ppu_on_all();
while(1);

//prep_3rd label starts
prep_3rd:
pal_col(0,0x16);
pal_col(1,0x20);
put_str(NTADR(12,13),"3RD STAGE");
put_str(NTADR(10,16),"OUT OF CONTROL");
ppu_on_all();
screen_fade_in();
delay(600);
screen_fade_out(5);
ppu_off();
set_vram_update(0,0);
vram_adr(0x2000);
vram_fill(0,1024);
goto game;
ppu_on_all();
while(1);

//prep_4th label starts
prep_4th:
pal_col(0,0x0);
pal_col(1,0x20);
put_str(NTADR(11,13),"FINAL STAGE");
put_str(NTADR(13,16),"CRASH");
ppu_on_all();
screen_fade_in();
delay(600);
screen_fade_out(5);
ppu_off();
set_vram_update(0,0);
vram_adr(0x2000);
vram_fill(0,1024);
goto game;
ppu_on_all();
while(1);

//game label starts
game:
music_play(music_battle);
ppu_on_all();
screen_fade_in();
put_str(NTADR(3,3),"HELLO");
ppu_on_all();
while(1){
ppu_waitnmi(); //wait for next TV frame
if (frame>=100){if (stage==2){time-=rand8()%2+1;} else {time-=1; } 
frame=0;
boss.x=rand8();
boss.y=rand8();} else {frame+=1; } 
if (time==0){music_stop();
ppu_off();set_vram_update(0,0);vram_adr(0x2000);vram_fill(0,1024);goto game_over;} 
sprite(&boss);
memcpy(game_nmi_list,game_nmi_init,sizeof(game_nmi_init));set_vram_update(24,game_nmi_list);game_nmi_list[2]=0x34;game_nmi_list[5]=0x29;game_nmi_list[8]=0x2d;game_nmi_list[11]=0x25;game_nmi_list[14]=0x1a;game_nmi_list[17]=0x10+ time/10%10;game_nmi_list[20]=0x10+ time%10;game_nmi_list[23]=0;game_nmi_list[26]=0x33;game_nmi_list[29]=0x23;game_nmi_list[32]=0x2f;game_nmi_list[35]=0x32;game_nmi_list[38]=0x25;game_nmi_list[41]=0x1a;game_nmi_list[44]=0x10+ score/10%10;game_nmi_list[47]=0x10+ score%10;game_nmi_list[50]=0;game_nmi_list[53]=0x33;game_nmi_list[56]=0x30;game_nmi_list[59]=0x25;game_nmi_list[62]=0x25;game_nmi_list[65]=0x24;game_nmi_list[68]=0x1a;game_nmi_list[71]=0x10+ speed/3;
if (stage<=3){if (is_pressed(PAD_LEFT)&&player.x>0){player.x-=2;} 
if (is_pressed(PAD_RIGHT)&&player.x<232){player.x+=2;} } else {delta=rand8();
if (delta%2){player.x-=delta%3;} 
delta=rand8();
if (delta%2){player.x+=delta%3;}  } 
sprite(&player);
if (is_pressed(PAD_A)&&is_attacking==0){is_attacking=1;
mountain.x=player.x;
mountain.y=220;
if (stage>=3){speed=rand8()%5;} } 
if (is_attacking>0){is_attacking+=1;
mountain.y-=speed;
if (mountain.y>0){if (mountain.y>=boss.y&&mountain.y<=boss.y+16&&mountain.x>=boss.x&&mountain.x<=boss.x+24){sfx_play(SFX_ATTACK,0);
is_attacking=0;
star.x=mountain.x-4;
star.y=mountain.y;
sprite(&star);
score+=6-(speed/3);
if (score>100){music_stop();
time=15;
score=0;
frame=101;
stage+=1;
if (stage==2){ppu_off();set_vram_update(0,0);vram_adr(0x2000);vram_fill(0,1024);goto prep_2nd;} else if (stage==3){ppu_off();set_vram_update(0,0);vram_adr(0x2000);vram_fill(0,1024);goto prep_3rd;} else if (stage==4){ppu_off();set_vram_update(0,0);vram_adr(0x2000);vram_fill(0,1024);goto prep_4th;} else {ppu_off();set_vram_update(0,0);vram_adr(0x2000);vram_fill(0,1024);goto game_clear; } } } else {sprite(&mountain); } } else {is_attacking=0; } } else {if (is_pressed(PAD_UP)&&speed<15){speed+=1;} else if (is_pressed(PAD_DOWN)&&speed>1){speed-=1;}  } 
}
ppu_on_all();
while(1);

//game_over label starts
game_over:
pal_col(0,0x25);
pal_col(1,0x20);
put_str(NTADR(2,2),"YOU LOSE");
sfx_play(SFX_FINISH,0);
ppu_on_all();
while(1);

//game_clear label starts
game_clear:
pal_col(0,0x22);
pal_col(1,0x20);
put_str(NTADR(2,2),"YOU WIN");
sfx_play(SFX_FINISH,0);
ppu_on_all();
while(1);
}

