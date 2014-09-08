battle:
	.word @chn0,@chn1,@chn2,@chn3,@chn4,music_instruments
	.byte $03

@chn0:
@chn0_0:
	.byte $c1,$42
@chn0_segno:
	.byte $1c,$84,$3f,$90,$95,$3f,$1c,$84,$3f,$90,$95,$3f,$1d,$84,$3f,$90,$95,$3f,$1d,$84,$3f,$90
	.byte $fe
	.word @chn0_segno
@chn0_loop:
@chn0_1:
	.byte $9f
	.byte $fe
	.word @chn0_loop

@chn1:
@chn1_0:
	.byte $c1,$43
@chn1_segno:
	.byte $07,$95,$3f,$95,$3f,$05,$95,$3f,$95,$3f,$04,$95,$3f,$95,$3f,$02,$95,$3f
	.byte $fe
	.word @chn1_segno
@chn1_loop:
@chn1_1:
	.byte $9f
	.byte $fe
	.word @chn1_loop

@chn2:
@chn2_0:

	.byte $89
@chn2_loop:
@chn2_1:
	.byte $9f
	.byte $fe
	.word @chn2_loop

@chn3:
@chn3_0:

@chn3_loop:
@chn3_1:
	.byte $9f
	.byte $fe
	.word @chn3_loop

@chn4:
@chn4_0:

@chn4_loop:
@chn4_1:
	.byte $9f
	.byte $fe
	.word @chn4_loop
