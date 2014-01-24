require 'Player'

class PlayerParser
	def parse(input)
		player = Player.new
		player.name, cards_literal = input.split ": "
		player.cards = cards_literal.split.map { |e| Card.new e}
		
		player
	end
end