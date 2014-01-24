require 'player'

class PlayerParser
	def parse(input)
		player = Player.new
		player.name, cards_literal = input.split ": "
		player.cards = parse_cards(cards_literal)
		
		player
	end

	def parse_cards(cards_literal)
		cards_literal.split.map { |e| Card.new e}
	end
end