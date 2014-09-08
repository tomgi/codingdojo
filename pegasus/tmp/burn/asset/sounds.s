sounds:
	.word @change
	.word @attack
	.word @finish
@change:
	.byte $00,$af
	.byte $01,$c8
	.byte $14
	.byte $00,$af
	.byte $01,$c8
	.byte $14
	.byte $01,$64
	.byte $13
	.byte $ff
@attack:
	.byte $00,$af
	.byte $01,$c8
	.byte $19
	.byte $00,$af
	.byte $01,$c8
	.byte $19
	.byte $01,$64
	.byte $1a
	.byte $00,$af
	.byte $01,$c8
	.byte $19
	.byte $01,$64
	.byte $1a
	.byte $01,$32
	.byte $1a
	.byte $ff
@finish:
	.byte $00,$aa
	.byte $01,$5a
	.byte $23
	.byte $00,$aa
	.byte $01,$5a
	.byte $23
	.byte $01,$64
	.byte $1e
	.byte $00,$aa
	.byte $01,$5a
	.byte $23
	.byte $01,$64
	.byte $1e
	.byte $01,$5a
	.byte $1e
	.byte $ff
