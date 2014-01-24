require 'rspec'
require_relative '../lib/poker_hands_app'
require_relative '../lib/player_parser'

describe "PokerHandsApp" do
	context "AcceptanceTests" do
		it "generates correct results for high card ace" do
			ph = PokerHandsApp.new
			result = ph.compare_hands("Black: 2H 3D 5S 9C KD White: 2C 3H 4S 8C AH")
			result.should == "White wins - high card: Ace"
		end
	end

	context "Input Parsing" do
		context "parsing single player" do
			let(:parser) { PlayerParser.new }
			let(:player) { parser.parse("White: 2C 3H 4S 8C AH") }

			it "should parse player name" do 
				player.name.should == "White"
			end

			it "should parse first cards correctly" do
				expectedCards = [
					Card.new('2C'),
					Card.new('3H'),
					Card.new('4S'),
					Card.new('8C'),
					Card.new('AH')
				]

				player.cards.should =~ expectedCards
			end

		end
	end

	context "get rank" do
		it "should recognize high card" do
			player = Player.new	
			player.cards = [
				Card.new('2C'),
				Card.new('3H'),
				Card.new('4S'),
				Card.new('8C'),
				Card.new('AH')
			]
			player.rank.name.should == "High card"
			player.rank.highest_card.should == Card.new('AH')
		end
	end

	context "comparing ranks" do
		it "should recognize highest rank" do
			player1 = Player.new "white"
			player1.cards = [
				Card.new('2C'),
				Card.new('3H'),
				Card.new('4S'),
				Card.new('8C'),
				Card.new('AH')
			]
			player2 = Player.new "black"
			player2.cards = [
				Card.new('2H'),
				Card.new('3D'),
				Card.new('5S'),
				Card.new('9C'),
				Card.new('KD')
			]

			player1.rank.should > player2.rank
		end
	end

	context Rank do
		it "should have correct string format" do
			r = Rank.new "High card", Card.new("AD")
			r.to_s.should == "high card: Ace"
		end
	end
end