require 'card'
require 'rank'
require 'ranks/factory'

class Player
	attr_accessor :name
	attr_accessor :cards

	def initialize(name = "")
		@name = name
	end

	def cards= value
		@rank = Factory.new.create_rank value
		@cards = value
	end

	def rank
		@rank	
	end
end