sarabande:
	.word @chn0,@chn1,@chn2,@chn3,@chn4,music_instruments
	.byte $03

@chn0:
@chn0_0:
	.byte $c2,$43,$1a,$89,$3f,$89,$3f,$1a,$89,$3f,$95,$3f,$1a,$89,$3f,$15,$89,$3f,$89,$3f,$15,$89,$3f,$16,$89,$3f,$15,$89,$3f,$13,$89,$3f,$11,$89,$3f,$89,$3f,$11,$89,$3f,$95,$3f,$11,$89,$3f,$18,$89,$3f,$89,$3f,$18,$89,$3f,$18,$89,$3f,$16,$89,$3f,$15,$89,$3f,$13,$89,$3f,$89,$3f,$13,$89,$3f,$95,$3f
@chn0_loop:
@chn0_1:
	.byte $9f
	.byte $fe
	.word @chn0_loop

@chn1:
@chn1_0:
	.byte $c2,$42,$29,$89,$3f,$89,$3f,$29,$89,$3f,$95,$3f,$2b,$89,$3f,$28,$89,$3f,$89,$3f,$28,$89,$3f,$89,$3f,$95,$3f,$2d,$89,$3f,$89,$3f,$2d,$89,$3f,$95,$3f,$2e,$89,$3f,$2b,$89,$3f,$89,$3f,$2b,$89,$3f,$95,$3f,$2d,$89,$3f,$2e,$89,$3f,$89,$3f,$2e,$89,$3f,$95,$3f
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
